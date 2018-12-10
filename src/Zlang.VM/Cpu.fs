namespace Zlang.VM

open System
open System.Collections.Generic

open Zlang.Bytecode.Opcodes
open Zlang.Bytecode.Instructions
open System.Diagnostics

module Cpu =
    type Register =
        | R0 | A
        | R1 | B
        | R2 | C
        | R3 | D
        | R4 | E
        | R5 | F
        | R6 | G
        | R7 | H
        | R8
        | R9
        | R10
        | R11
        | R12
        | R13 | FL
        | R14 | SP
        | R15 | PC

    type registers = {
        mutable r0: int;
        mutable r1: int;
        mutable r2: int;
        mutable r3: int;
        mutable r4: int;
        mutable r5: int;
        mutable r6: int;
        mutable r7: int;
        mutable r8: int;
        mutable r9: int;
        mutable r10: int;
        mutable r11: int;
        mutable r12: int;
        mutable r13: int;
        mutable r14: int;
        mutable r15: int;
    }

    let get_zero_registers =
        {r0=0;r1=0;r2=0;r3=0;r4=0;r5=0;r6=0;r7=0;r8=0;r9=0;r10=0;r11=0;r12=0;r13=0;r14=0;r15=0}

    type result =
        | Success
        | Halt
        | Error of string

    type VirtualMachine (?mem_size: int) =
        let registers =
            get_zero_registers

        let memory =
            (Array.init
                <| match mem_size with
                    | None -> 32
                    | Some i -> i)
                    <| fun _ -> 0

        let get_value address =
            memory.[address]
        let set_value address v =
            memory.[address] <- v

        let stack =
            new Stack<int> ()

        let get_register (id) =
            match id with
            | R0 | A -> registers.r0
            | R1 | B -> registers.r1
            | R2 | C -> registers.r2
            | R3 | D -> registers.r3
            | R4 | E -> registers.r4
            | R5 | F -> registers.r5
            | R6 | G -> registers.r6
            | R7 | H -> registers.r7
            | R8 -> registers.r8
            | R9 -> registers.r9
            | R10 -> registers.r10
            | R11 -> registers.r11
            | R12 -> registers.r12
            | R13 | FL -> registers.r13
            | R14 | SP -> registers.r14
            | R15 | PC -> registers.r15

        let set_register id v =
            match id with
            | R0 | A -> registers.r0 <- v
            | R1 | B -> registers.r1 <- v
            | R2 | C -> registers.r2 <- v
            | R3 | D -> registers.r3 <- v
            | R4 | E -> registers.r4 <- v
            | R5 | F -> registers.r5 <- v
            | R6 | G -> registers.r6 <- v
            | R7 | H -> registers.r7 <- v
            | R8 -> registers.r8 <- v
            | R9 -> registers.r9 <- v
            | R10 -> registers.r10 <- v
            | R11 -> registers.r11 <- v
            | R12 -> registers.r12 <- v
            | R13 | FL -> registers.r13 <- v
            | R14 | SP -> registers.r14 <- v
            | R15 | PC -> registers.r15 <- v

        let register_of_addr addr =
            match addr with
            | 0x00 -> R0
            | 0x01 -> R1
            | 0x02 -> R2
            | 0x03 -> R3
            | 0x04 -> R4
            | 0x05 -> R5
            | 0x06 -> R6
            | 0x07 -> R7
            | 0x08 -> R8
            | 0x09 -> R9
            | 0x0A -> R10
            | 0x0B -> R11
            | 0x0C -> R12
            | 0x0D -> R13
            | 0x0E -> R14
            | 0x0F -> R15
            | _ -> addr |> failwithf "Invalid register addr: 0x%02X"

        member this.fetch =
            if (get_register PC) = memory.Length then
                failwith "End of Memory"
            let buf = memory.[get_register PC] in
            ((get_register PC) + 1) |> set_register PC
            buf;

        member this.load_program prog address =
            match prog with
            | cur :: rest -> memory.[address] <- cur; this.load_program rest (address + 1)
            | [] -> ()

        member this.decode_instruction value =
            let res = get_opcode value in
            match fst res with
            | false -> (sprintf "Invalid opcode: 0x%02X" value) |> Exception |> raise
            | true ->
                instruction_of_opcode <| snd res

        member this.run_instruction (instr: Instruction): result =
            try
                match instr.opcode with
                | Opcode.Nop -> Success
                | Opcode.Pop -> stack.Pop() |> ignore; Success
                | Opcode.Dup -> stack.Peek() |> stack.Push; Success
                | Opcode.Load_c -> this.fetch |> stack.Push; Success
                | Opcode.Add -> stack.Pop () + stack.Pop () |> stack.Push; Success
                | Opcode.Sub -> stack.Pop () - stack.Pop () |> stack.Push; Success
                | Opcode.Mul -> stack.Pop () * stack.Pop () |> stack.Push; Success
                | Opcode.Div -> stack.Pop () / stack.Pop () |> stack.Push; Success
                | Opcode.Halt -> Halt
                | _ -> (sprintf "Undefined opcode: %s\n" <| Opcode.GetName (typeof<Opcode>, instr.opcode)) |> Error
            with
                | Failure (e) -> e |> Error

        member this.run =
            match this.decode_instruction this.fetch
                |> this.run_instruction with
                | Error (e) -> e |> failwithf "[Error] %s"
                | Halt -> stack.Peek () |> printfn "[Halt] Top of stack: 0x%02X"
                | Success -> this.run
