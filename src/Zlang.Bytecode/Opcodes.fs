namespace Zlang.Bytecode

module Opcodes =
    [<Literal>]
    let Nop = 0x00

    [<Literal>]
    let Pop = 0x01

    [<Literal>]
    let Dup = 0x02

    [<Literal>]
    let Load_c = 0x10

    [<Literal>]
    let Load_r = 0x11

    [<Literal>]
    let Store_c = 0x20

    [<Literal>]
    let Store_s = 0x21

    [<Literal>]
    let Add = 0x30

    [<Literal>]
    let Sub = 0x31

    [<Literal>]
    let Mul = 0x32

    [<Literal>]
    let Div = 0x33

    [<Literal>]
    let Jump = 0x40

    [<Literal>]
    let Jump_z = 0x41

    [<Literal>]
    let Jump_nz = 0x42

    [<Literal>]
    let Halt = 0xFF
