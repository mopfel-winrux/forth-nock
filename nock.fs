\ Structure:
\ First word: 0 for atom, 1 for cell
\ Second word: For atoms: the value
\             For cells: pointer to head
\ Third word: For cells: pointer to tail

: field+ ( addr -- addr' ) cell + ;  \ Move to next field

: make-cell ( head tail -- addr )  \ Create a cell from two nouns
    swap
    here >r          \ Save current position
    1 ,             \ Tag as cell (1)
    , ,             \ Store tail and head
    r> ;            \ Return address of cell

: make-atom ( n -- addr )  \ Create an atom
    here >r         \ Save current position
    0 ,            \ Tag as atom (0)
    ,              \ Store value
    r> ;           \ Return address of atom

: is-cell? ( addr -- flag ) @ 1 = ;
: get-value ( addr -- n ) field+ @ ;
: get-head ( addr -- addr ) field+ @ ;
: get-tail ( addr -- addr ) field+ field+ @ ; 

DEFER .NOUN  \ Forward declaration

: print-noun ( addr -- )  \ Print a noun
    dup is-cell?    \ Check if cell
    if             \ If cell
        ." ["
        dup get-head recurse  \ Print head
        ." " 
        get-tail recurse  \ Print tail
        ." ]"
    else           \ If atom
        get-value    \ Get value
        .          \ Print it
    then ;

: .noun-with-dup ( addr -- addr )  \ Wrapper that ensures dup before printing
    dup print-noun ;

' .noun-with-dup IS .NOUN  \ Resolve the deferred word to include dup

\ === Begin Nock Implementation ===

\ ? operator (wut) - checks if noun is atom
: wut ( addr -- n )
    is-cell? IF 0 ELSE 1 THEN ;

\ + operator (lus) - increment atom
: lus ( addr -- addr )
    .S
    dup is-cell? IF
        \ If cell, return as is
        dup
    ELSE
        \ If atom, increment value
        dup get-value 1+ make-atom
    THEN ;

\ = operator (tis) - equality test
: tis ( addr addr -- n )
    \ Recursive equality check would go here
    \ For now just compare addresses
    = IF 0 ELSE 1 THEN ;

\ / operator (slot) - tree addressing
DEFER slot  \ Forward declaration for recursion

: do-slot ( n addr -- addr )
    swap                    \ Get n on top
    dup 1 = IF             \ Case /1
        drop
    ELSE dup 2 = IF        \ Case /2
        drop get-head
    ELSE dup 3 = IF        \ Case /3
        drop get-tail
    ELSE
        \ Handle deeper paths recursively
        dup 2 MOD 0= IF    \ Even path
            2/ swap slot 2 swap slot
        ELSE               \ Odd path
            1- 2/ swap slot 3 swap slot
        THEN
    THEN THEN THEN ;

' do-slot IS slot  \ Resolve the deferred word

\ * operator (tar) - main reduction engine
DEFER tar  \ Forward declaration for recursion

: nock-0 ( addr -- addr )  \ [a 0 b] → /[1 + b]a
    get-tail get-tail get-value swap slot ;

: nock-1 ( addr -- addr )  \ [a 1 b] → b
    swap drop
    get-tail get-tail ;

: nock-2 ( addr -- addr )  \ [a 2 b c] → *[*[a b] *[a c]]
    dup get-tail get-tail get-head 
    >R swap R> make-cell tar swap
    dup get-head swap
    get-tail get-tail get-tail make-cell tar
    make-cell
    tar ;

: nock-3 ( addr -- addr )  \ [a 3 b] → ?*[a b]
    get-tail get-tail make-cell tar wut make-atom ;

: nock-4 ( addr -- addr )  \ [a 4 b] → +*[a b]
    get-tail get-tail make-cell tar lus ;

: nock-5 ( addr -- addr )  \ [a 5 b] → =*[a b]
    \ TODO: Implement equals operation
    dup ;

: test-tar ( addr -- addr )
    dup get-tail get-head get-value  \ Get operation number
    swap dup get-head swap rot
    ;

: do-tar ( addr -- addr )
    dup get-tail get-head get-value  \ Get operation number
    swap dup get-head swap rot
    \ TODO: Check for Autocons
    CASE
        0 OF nock-0 ENDOF
        1 OF nock-1 ENDOF
        2 OF nock-2 ENDOF
        3 OF nock-3 ENDOF
        4 OF nock-4 ENDOF
        5 OF nock-5 ENDOF
        \ TODO: Add cases 6-10
    ENDCASE 
    cr .noun ;

' do-tar IS tar  \ Resolve the deferred word

: nock ( addr -- addr )  \ Main entry point
    tar ;

\ Helper words for testing
: mk-slot \ Creates slot formula [0 n]
    0 make-atom
    1 make-atom
    make-cell ;

: mk-constant
    1 make-atom
    42 make-atom
    make-cell ;

: mk-cell
    3 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell ;

: mk-inc  \ Creates increment formula [4 0 1]
    4 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell ;

: mk-eval  \ Creates  [2 [0 3] [1 [4 0 1]]
    2 make-atom
    0 make-atom
    3 make-atom
    make-cell
    1 make-atom
    mk-inc
    make-cell
    make-cell 
    make-cell ;


\ Creates [[4 5] [6 14 15]]
: test-noun
    4 make-atom
    5 make-atom
    make-cell
    6 make-atom
    14 make-atom
    15 make-atom
    make-cell
    make-cell
    make-cell
    ;
    
: test-slot
    42 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell ;

: test-eval
    50 make-atom
    51 make-atom
    make-cell
    mk-eval
    make-cell ;

: test-inc
    test-noun
    4 make-atom
    0 make-atom
    15 make-atom
    make-cell
    make-cell
    make-cell ;

: test-cell
    42 make-atom
    43 make-atom
    make-cell
    mk-cell
    make-cell
    ;
