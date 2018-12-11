namespace Zlang.Bytecode

open Tokens

module Disassembler =

    type state = { mutable pc: int; dump: int list }

    let fetch st = 
        if st.pc = st.dump.Length then
            failwith "EOF"
        let word = List.item st.pc st.dump in
        st.pc <- st.pc + 1;
        word;

    let rec tokenize state =
        let inline fetch () = fetch state in
        try
            match fetch () with
                | Opcodes.Nop -> Instruction "Nop" :: []
                | Opcodes.Pop -> Instruction "Pop" :: []
                | Opcodes.Dup -> Instruction "Dup" :: []
                | Opcodes.Load_c -> Instruction "Load.c" :: Value (fetch ()) :: []
                | Opcodes.Load_r -> Instruction "Load.r" :: Register (fetch ()) :: []
                | Opcodes.Store_c -> Instruction "Store.c" :: Register (fetch ()) :: Value (fetch ()) :: []
                | Opcodes.Store_s -> Instruction "Store.s" :: Register (fetch ()) :: []
                | Opcodes.Add -> Instruction "Add" :: []
                | Opcodes.Sub -> Instruction "Sub" :: []
                | Opcodes.Mul -> Instruction "Mul" :: []
                | Opcodes.Div -> Instruction "Div" :: []
                | Opcodes.Jump -> Instruction "Jump" :: Address (fetch ()) :: []
                | Opcodes.Jump_z -> Instruction "Jump.z" :: Address (fetch ()) :: []
                | Opcodes.Jump_nz -> Instruction "Jump.nz" :: Address (fetch ()) :: []
                | Opcodes.Halt -> Instruction "Halt" :: []
                | e -> Unknown e :: []
            @ tokenize state
        with
        | Failure e when e = "EOF" -> EOF :: []

    let format token =
        match token with
        | Instruction i -> i 
        | Value v -> sprintf "%d" v
        | Register r -> sprintf "R%d" r
        | Address a -> sprintf "0x%02X" a
        | Unknown u -> sprintf "Unknown value: 0x%02X" u
        | EOF -> ""

    let rec source_of_tokens (tokens: token list) : string =
        match tokens.Head with
        | Instruction i -> sprintf "\n%s\t%s" (format tokens.Head) (source_of_tokens tokens.Tail)
        | EOF -> format EOF
        | t -> sprintf "%s\t%s" (format t) (source_of_tokens tokens.Tail)

    let run words =
        { pc = 0; dump = words }
            |> tokenize
            |> source_of_tokens
