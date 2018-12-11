namespace Zlang.VM

open System.Collections.Generic
open System
open Zlang.Bytecode

module Zvm =
    type Zvm =
        {
            registers: Registers.registers;
            stack: Stack<int>;
            memory: int[];
        }

    type run_result =
        | Success
        | Halt
        | Error of string

    let init mem_size =
        {
            registers = Registers.init;
            stack = new Stack<int>();
            memory = Array.init mem_size <| fun  _ -> 0;
        }

    let rec load_dump dump address vm =
        match dump with
        | b :: rest -> vm.memory.[address] <- b; load_dump rest (address + 1) vm
        | [] -> ()

    let get_register vm =
        Registers.get_register vm.registers

    let set_register vm =
        Registers.set_register vm.registers

    let fetch vm =
        let pc = get_register vm Registers.PC in
        if pc = vm.memory.Length then
            failwith "Out of memory"
        let buf = vm.memory.[pc]
        set_register vm Registers.PC <| pc + 1
        buf

    let run_instruction vm opcode =
        let stack = vm.stack in
        let inline fetch () = fetch vm in
        let inline register_of_addr v = Registers.register_of_addr v in
        let inline set_register r v = Registers.set_register vm.registers r v in
        let inline get_register r = Registers.get_register vm.registers r in

        try
            match opcode with
            | Opcodes.Nop -> Success;
            | Opcodes.Pop -> stack.Pop() |> ignore; Success
            | Opcodes.Dup -> stack.Peek() |> stack.Push; Success
            | Opcodes.Load_c -> fetch () |> stack.Push; Success
            | Opcodes.Load_r -> fetch () |> register_of_addr |> get_register |> stack.Push; Success
            | Opcodes.Store_c -> fetch () |> register_of_addr |> set_register <| fetch (); Success
            | Opcodes.Store_s -> fetch () |> register_of_addr |> set_register <| stack.Pop (); Success
            | Opcodes.Add -> stack.Pop () + stack.Pop () |> stack.Push; Success
            | Opcodes.Sub -> stack.Pop () - stack.Pop () |> stack.Push; Success
            | Opcodes.Mul -> stack.Pop () * stack.Pop () |> stack.Push; Success
            | Opcodes.Div -> stack.Pop () / stack.Pop () |> stack.Push; Success
            | Opcodes.Jump -> fetch () |> set_register Registers.PC; Success
            | Opcodes.Jump_z ->
                let br = fetch () in
                if stack.Peek() = 0 then
                    set_register Registers.PC br
                Success
            | Opcodes.Jump_nz ->
                let br = fetch () in
                if stack.Peek() <> 0 then
                    set_register Registers.PC br
                Success
            | Opcodes.Halt -> Halt
            | _ -> failwithf "Undefined opcode: 0x%02X" opcode
        with
        | Failure (f) -> Error (f)

    let rec run vm =
        match fetch vm |> run_instruction vm with
        | Error (e) -> printfn "[Error] %s" e
        | Halt -> printfn "[Halt] top of stack: 0x%02X" <| vm.stack.Peek ()
        | Success -> run vm
