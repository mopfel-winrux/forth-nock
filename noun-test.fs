include nock.fs

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

: mk-equal \ Creates [5 [4 0 2] [0 3]]
    5 make-atom
    4 make-atom
    0 make-atom
    2 make-atom
    make-cell make-cell
    0 make-atom
    3 make-atom
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

: test-equal
    50 make-atom
    51 make-atom
    make-cell
    mk-equal
    make-cell
    ;

: test-auto \ [50 [[0 1] [1 203]]]
    50 make-atom
    0 make-atom
    1 make-atom
    make-cell
    1 make-atom
    203 make-atom 
    make-cell
    make-cell
    make-cell
    ;

: test-if \ [1 [6 [0 1] [0 1] [4 0 1]]]
    1 make-atom
    6 make-atom
    0 make-atom
    1 make-atom
    make-cell
    0 make-atom
    1 make-atom
    make-cell
    4 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell ;


: test-comp \ [42 [7 [4 0 1] [4 0 1]]]
    42 make-atom
    7 make-atom
    4 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell
    4 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell ;

: test-varadd \ [[67 39] [8 [0 3] [4 0 2]]]
    67 make-atom
    39 make-atom make-cell
    8 make-atom
    0 make-atom
    3 make-atom
    make-cell
    4 make-atom
    0 make-atom
    2 make-atom
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell ;

: test-core \ [45 [9 2 [1 4 0 3] 0 1]]
    45 make-atom
    9 make-atom
    2 make-atom
    1 make-atom
    4 make-atom
    0 make-atom
    3 make-atom
    make-cell make-cell make-cell
    0 make-atom
    1 make-atom make-cell
    make-cell
    make-cell
    make-cell
    make-cell
    ;

: test-replace \ [50 [10 [2 [0 1]] [1 8 9 10]]]
    50 make-atom
    10 make-atom
    2 make-atom
    0 make-atom
    1 make-atom
    make-cell
    make-cell
    1 make-atom
    8 make-atom
    9 make-atom
    10 make-atom
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell
    make-cell ;

\ *[[*[a c] *[a d]] 0 3]

: test-dynamic-hint \ [[50 51] [11 [369 [1 20]] 0 2]]
    50 make-atom
    51 make-atom
    make-cell
    11 make-atom
    369 make-atom
    1 make-atom
    20 make-atom
    make-cell
    make-cell
    0 make-atom
    2 make-atom
    make-cell
    make-cell
    make-cell
    make-cell ;

: test-static-hint \ [[50 51] [11 369 0 2]]
    50 make-atom
    51 make-atom
    make-cell
    11 make-atom
    369 make-atom
    0 make-atom
    2 make-atom
    make-cell
    make-cell
    make-cell
    make-cell ;

: eq
    50 make-atom
    51 make-atom
    make-cell
    50 make-atom
    51 make-atom
    make-cell ;

