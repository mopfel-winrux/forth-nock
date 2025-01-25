\ Test suite for nock
include nock.fs

\ Creates [[4 5] [6 14 15]]
: make-subject
    4 make-atom
    5 make-atom
    make-cell
    6 make-atom
    14 make-atom
    15 make-atom
    make-cell
    make-cell
    make-cell ;

: show-test
   .noun ." -> "
   nock
   .noun ;

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
    show-test ;

: test-slot
    make-subject
    0 make-atom
    1 make-atom
    make-cell make-cell
    show-test ; 

: test-constant
    make-subject
    1 make-atom
    0 make-atom
    make-cell make-cell
    show-test ;

: test-eval
    50 make-atom
    51 make-atom
    make-cell
    2 make-atom
    0 make-atom
    3 make-atom
    make-cell
    1 make-atom
    4 make-atom
    0 make-atom
    1 make-atom
    make-cell make-cell make-cell
    make-cell make-cell make-cell
    show-test ;

: test-cell
    make-subject
    3 make-atom
    0 make-atom
    4 make-atom
    make-cell make-cell make-cell
    show-test ;

: test-inc
    make-subject
    4 make-atom
    0 make-atom
    15 make-atom
    make-cell make-cell make-cell
    show-test ;

: test-eq
    make-subject
    5 make-atom
    4 make-atom
    0 make-atom
    15 make-atom
    make-cell make-cell
    0 make-atom
    3 make-atom
    make-cell make-cell make-cell
    make-cell
    show-test ;

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
    make-cell make-cell make-cell
    make-cell make-cell make-cell 
    show-test ;

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
    make-cell make-cell make-cell make-cell make-cell
    show-test ;

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
    make-cell make-cell make-cell make-cell make-cell
    show-test ;

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
    make-cell make-cell make-cell make-cell
    show-test ;

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
    make-cell make-cell make-cell 
    make-cell make-cell make-cell 
    show-test ;

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
    make-cell make-cell make-cell make-cell
    show-test ;

: test-static-hint \ [[50 51] [11 369 0 2]]
    50 make-atom
    51 make-atom
    make-cell
    11 make-atom
    369 make-atom
    0 make-atom
    2 make-atom
    make-cell make-cell make-cell make-cell 
    show-test ;

: run-tests
    cr
    ." test-auto: " test-auto cr
    ." test-slot: " test-slot cr
    ." test-constant: " test-constant cr
    ." test-eval: " test-eval cr
    ." test-cell: " test-cell cr
    ." test-inc: " test-inc cr
    ." test-eq: " test-eq cr
    ." test-if: " test-if cr
    ." test-comp: " test-comp cr
    ." test-varadd: " test-varadd cr
    ." test-core: " test-core cr
    ." test-replace: " test-replace cr
    ." test-dynamic-hint: " test-dynamic-hint cr
    ." test-static-hint: " test-static-hint cr
    ;

