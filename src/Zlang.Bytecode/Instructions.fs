namespace Zlang.Bytecode
open Zlang.Bytecode.Opcodes

module Instructions =
    type Instruction = {
        opcode: Opcode;
        mnemonic: string;
        arg_size: int;
    };

    (* Base instructions *)
    let Nop =
        { opcode = Opcode.Nop; mnemonic = "Nop"; arg_size = 0 }

    (* Stack manipulate instructions *)
    let Pop =
        { opcode = Opcode.Pop; mnemonic = "Pop"; arg_size = 0 }
    let Dup =
        { opcode = Opcode.Dup; mnemonic = "Dup"; arg_size = 0 }

    let Loadc =
        { opcode = Opcode.Load_c; mnemonic = "Loadc"; arg_size = 1 }

    (* Stack arithmetic instructions *)
    let Add =
        { opcode = Opcode.Add; mnemonic = "Add"; arg_size = 0 }
    let Sub =
        { opcode = Opcode.Sub; mnemonic = "Sub"; arg_size = 0 }
    let Mul =
        { opcode = Opcode.Mul; mnemonic = "Mul"; arg_size = 0 }
    let Div =
        { opcode = Opcode.Div; mnemonic = "Div"; arg_size = 0 }
    let Halt =
        { opcode = Opcode.Halt; mnemonic = "Halt"; arg_size = 0 }

    let instruction_of_opcode opcode =
        match opcode with
        | Opcode.Nop -> Nop
        | Opcode.Pop -> Pop
        | Opcode.Dup -> Dup
        | Opcode.Load_c -> Loadc
        | Opcode.Add -> Add
        | Opcode.Sub -> Sub
        | Opcode.Mul -> Mul
        | Opcode.Div -> Div
        | Opcode.Halt -> Halt