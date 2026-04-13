\ Nock interpreter in North Forth

\ ── Memory layout for nouns ──────────────────────────────────────────────
\ Atoms:  [tag=0, value]          (2 cells)
\ Cells:  [tag=1, head-addr, tail-addr]  (3 cells)

: field+  ( addr -- addr' )  CELL+ ;   \ advance by one cell (cell size = 1)

: make-cell  ( head tail -- addr )
    SWAP
    HERE >R
    1 ,
    , ,
    R> ;

: make-atom  ( n -- addr )
    HERE >R
    0 ,
    ,
    R> ;

: is-cell?  ( addr -- flag )  @ 1 = IF 1 ELSE 0 THEN ;
: get-value  ( addr -- n )    field+ @ ;
: get-head   ( addr -- addr ) field+ @ ;
: get-tail   ( addr -- addr ) field+ field+ @ ;

DEFER .NOUN

: print-noun  ( addr -- )
    dup is-cell?
    IF
        [CHAR] [ EMIT
        dup get-head RECURSE
        BL EMIT
        get-tail RECURSE
        [CHAR] ] EMIT  BL EMIT
    ELSE
        get-value .
    THEN ;

: .noun-dup  ( addr -- addr )  dup print-noun ;
' .noun-dup IS .NOUN

\ ── Nock operators ───────────────────────────────────────────────────────

\ wut ?  — 0 if cell, 1 if atom
: wut  ( addr -- n )
    is-cell? IF 0 ELSE 1 THEN ;

\ lus +  — increment atom
: lus  ( addr -- addr )
    dup is-cell? IF
        CR .NOUN DROP 99 THROW
    ELSE
        get-value 1+ make-atom
    THEN ;

\ tis =  — equality (0=equal, 1=not-equal); result is an atom-noun addr
DEFER tis

: do-tis  ( addr1 addr2 -- addr )
    over is-cell? over is-cell? <> IF
        2drop 1 make-atom EXIT
    THEN
    over is-cell? IF
        2dup
        2dup get-head SWAP get-head tis
        get-value 0 = IF
            2drop get-tail SWAP get-tail tis
        ELSE
            2drop 2drop 1 make-atom
        THEN
    ELSE
        get-value SWAP get-value = IF 0 ELSE 1 THEN make-atom
    THEN ;

' do-tis IS tis

\ slot /  — tree addressing
DEFER slot

: do-slot  ( n-addr addr -- addr )
    SWAP get-value
    dup 1 = IF
        drop
    ELSE dup 2 = IF
        drop get-head
    ELSE dup 3 = IF
        drop get-tail
    ELSE
        dup 2 MOD 0 = IF
            2 / make-atom SWAP slot
            2 make-atom SWAP slot
        ELSE
            1- 2 / make-atom SWAP slot
            3 make-atom SWAP slot
        THEN
    THEN THEN THEN ;

' do-slot IS slot

\ hax #  — replace at axis
DEFER hax

: do-hax  ( n-addr new-val target -- addr )
    -ROT SWAP
    dup get-value 1 = IF
        drop nip
    ELSE
        get-value
        dup 2 MOD 0 = IF
            2 / >R
            SWAP dup
            R@ 2 * 1+ make-atom SWAP slot
            ROT SWAP make-cell
            SWAP R>
            make-atom -ROT
            hax
        ELSE
            1- 2 / >R
            SWAP dup
            R@ 2 * make-atom SWAP slot
            ROT make-cell
            SWAP R>
            make-atom -ROT
            hax
        THEN
    THEN ;

' do-hax IS hax

\ tar *  — main Nock reduction
DEFER tar

: autocons  ( subject [b c] -- [*[a b] *[a c]] )
    dup -ROT
    dup get-head SWAP get-tail get-head make-cell tar
    SWAP
    dup get-head SWAP get-tail get-tail make-cell tar
    make-cell ;

: nock-0  ( subject formula -- result )   \ [a 0 b] -> /[b a]
    get-tail get-tail SWAP slot ;

: nock-1  ( subject formula -- result )   \ [a 1 b] -> b
    SWAP DROP get-tail get-tail ;

: nock-2  ( subject formula -- result )   \ [a 2 b c] -> *[*[a b] *[a c]]
    dup get-tail get-tail get-head
    >R SWAP R> make-cell tar SWAP
    dup get-head SWAP
    get-tail get-tail get-tail make-cell tar
    make-cell tar ;

: nock-3  ( subject formula -- result )   \ [a 3 b] -> ?*[a b]
    get-tail get-tail make-cell tar wut make-atom ;

: nock-4  ( subject formula -- result )   \ [a 4 b] -> +*[a b]
    get-tail get-tail make-cell tar lus ;

: nock-5  ( subject formula -- result )   \ [a 5 b c] -> =*[a b] =*[a c]
    get-tail get-tail make-cell tar
    dup get-head SWAP get-tail
    tis ;

: nock-6  ( subject formula -- result )   \ [a 6 b c d] -> if *[a b] then *[a c] else *[a d]
    over over get-tail get-tail get-head make-cell tar
    get-value 0 = IF
        get-tail get-tail get-tail get-head make-cell tar
    ELSE
        get-tail get-tail get-tail get-tail make-cell tar
    THEN ;

: nock-7  ( subject formula -- result )   \ [a 7 b c] -> *[*[a b] c]
    dup -ROT
    get-tail get-tail get-head make-cell tar
    SWAP get-tail get-tail get-tail make-cell tar ;

: nock-8  ( subject formula -- result )   \ [a 8 b c] -> *[[*[a b] a] c]
    dup -ROT
    get-tail get-tail get-head make-cell tar
    over get-head make-cell
    SWAP get-tail get-tail get-tail
    make-cell tar ;

: nock-9  ( subject formula -- result )   \ [a 9 b c] -> *[*[a c] 2 [0 1] 0 b]
    dup -ROT
    get-tail get-tail get-tail make-cell tar
    SWAP get-tail get-tail get-head >R
    2 make-atom
    0 make-atom 1 make-atom make-cell
    0 make-atom R> make-cell
    make-cell make-cell make-cell
    tar ;

: nock-10  ( subject formula -- result )  \ [a 10 [b c] d] -> #[b *[a c] *[a d]]
    over
    over get-tail get-tail get-head
    dup get-head >R
    get-tail make-cell tar
    >R
    get-tail get-tail get-tail make-cell tar
    R> R> -ROT SWAP
    hax ;

: nock-11  ( subject formula -- result )  \ hint
    dup get-tail get-tail get-head
    is-cell? IF
        2dup
        get-tail get-tail get-head get-tail make-cell tar
        -ROT
        get-tail get-tail get-tail make-cell tar
        make-cell 0 make-atom 3 make-atom make-cell make-cell tar
    ELSE
        get-tail get-tail get-tail make-cell tar
    THEN ;

: do-tar  ( addr -- addr )
    dup get-tail get-head
    dup is-cell? IF
        drop autocons
    ELSE
        get-value
        SWAP dup get-head SWAP ROT
        dup 0 = IF  drop nock-0
        ELSE dup 1 = IF  drop nock-1
        ELSE dup 2 = IF  drop nock-2
        ELSE dup 3 = IF  drop nock-3
        ELSE dup 4 = IF  drop nock-4
        ELSE dup 5 = IF  drop nock-5
        ELSE dup 6 = IF  drop nock-6
        ELSE dup 7 = IF  drop nock-7
        ELSE dup 8 = IF  drop nock-8
        ELSE dup 9 = IF  drop nock-9
        ELSE dup 10 = IF  drop nock-10
        ELSE dup 11 = IF  drop nock-11
        ELSE
            drop 2drop 98 THROW
        THEN THEN THEN THEN THEN THEN
        THEN THEN THEN THEN THEN THEN
    THEN ;

' do-tar IS tar

: nock  ( addr -- addr )  tar ;
