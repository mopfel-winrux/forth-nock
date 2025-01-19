# forth-nock
Nock Interpreter written in Forth

`nock.fs` is the Nock implementation

`noun-test.fs` are some tests written

## Examples

I'm using `gforth` but any forth interperter should work

`$ gforth noun-test.fs`

Lets run the nock formula `[50 [[0 1] [1 203]]]` defined in `test-auto`

```
test-auto  ok 1
.noun [50 [[0 1 ][1 203 ]]] ok 1
```

`.noun` will display the noun at the top of the stack

You can execute any noun using the `nock` procedure

```
nock  ok 1
.noun [50 203 ] ok 1
```

## Todo

[] Implement Large atoms using a bigint library
[] Implement `+jam` and `+cue` in forth
[] Implement a noun parser in forth `[1 2 3]` -> noun


## Nock 4k Specification
```
Nock 4K

A noun is an atom or a cell.  An atom is a natural number.  A cell is an ordered pair of nouns.

Reduce by the first matching pattern; variables match any noun.

nock(a)             *a
[a b c]             [a [b c]]

?[a b]              0
?a                  1
+[a b]              +[a b]
+a                  1 + a
=[a a]              0
=[a b]              1

/[1 a]              a
/[2 a b]            a
/[3 a b]            b
/[(a + a) b]        /[2 /[a b]]
/[(a + a + 1) b]    /[3 /[a b]]
/a                  /a

#[1 a b]            a
#[(a + a) b c]      #[a [b /[(a + a + 1) c]] c]
#[(a + a + 1) b c]  #[a [/[(a + a) c] b] c]
#a                  #a

*[a [b c] d]        [*[a b c] *[a d]]

*[a 0 b]            /[b a]
*[a 1 b]            b
*[a 2 b c]          *[*[a b] *[a c]]
*[a 3 b]            ?*[a b]
*[a 4 b]            +*[a b]
*[a 5 b c]          =[*[a b] *[a c]]

*[a 6 b c d]        *[a *[[c d] 0 *[[2 3] 0 *[a 4 4 b]]]]
*[a 7 b c]          *[*[a b] c]
*[a 8 b c]          *[[*[a b] a] c]
*[a 9 b c]          *[*[a c] 2 [0 1] 0 b]
*[a 10 [b c] d]     #[b *[a c] *[a d]]

*[a 11 [b c] d]     *[[*[a c] *[a d]] 0 3]
*[a 11 b c]         *[a c]

*a                  *a
```
