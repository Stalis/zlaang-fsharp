namespace Zlang.Bytecode

module Opcodes =
    type opcode = int
    (* Base instructions *)
    [<Literal>]
    let Nop: opcode = 0x00

    [<Literal>]
    let Pop: opcode = 0x01

    [<Literal>]
    let Dup: opcode = 0x02

    (* Stack load instructions *)
    [<Literal>]
    let Load_c: opcode = 0x10

    [<Literal>]
    let Load_r: opcode = 0x11
    
    (* Register store instructions *)
    [<Literal>]
    let Store_c: opcode = 0x20

    [<Literal>]
    let Store_s: opcode = 0x21

    (* Math operations *)
    [<Literal>]
    let Add: opcode = 0x30

    [<Literal>]
    let Sub: opcode = 0x31

    [<Literal>]
    let Mul: opcode = 0x32

    [<Literal>]
    let Div: opcode = 0x33
    
    (* Branching *)
    [<Literal>]
    let Jump: opcode = 0x40

    [<Literal>]
    let Jump_z: opcode = 0x41

    [<Literal>]
    let Jump_nz: opcode = 0x42

    (* Service instructions *)
    [<Literal>]
    let Halt: opcode = 0xFF
