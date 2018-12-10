namespace Zlang.Bytecode

module Opcodes =
    type Opcode =
        | Nop = 0x00
        | Pop = 0x01
        | Dup = 0x02
        | Load_c = 0x10
        | Load_r = 0x11
        | Store_c = 0x20
        | Store_s = 0x21
        | Store_r = 0x22
        | Add = 0x30
        | Sub = 0x31
        | Mul = 0x32
        | Div = 0x33
        | Halt = 0xFF

    let get_opcode (v: int): bool * Opcode =
        Opcode.GetName(typeof<Opcode>, v)
            |> Opcode.TryParse<Opcode>
