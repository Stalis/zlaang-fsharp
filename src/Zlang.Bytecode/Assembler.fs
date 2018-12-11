namespace Zlang.Bytecode

open System
open Opcodes

module Assembler =

    type token =
        | Instruction of opcode
        | Register of int
        | Value of int
     
    let int_of_token t =
        match t with 
        | Instruction i -> int i
        | Register r -> r
        | Value v -> v
        
    let (|Prefix|_|) (p:string) (s:string) =
         if s.StartsWith(p) then
             Some(s.Substring(p.Length))
         else
             None
    
    let rec tokenizeWord (word: string) =
        match word.ToLower() with 
        | "nop" -> Instruction Nop
        | "pop" -> Instruction Pop
        | "dup" -> Instruction Dup
        | "load.c" -> Instruction Load_c
        | "load.r" -> Instruction Load_r
        | "store.c" -> Instruction Store_c
        | "store.s" -> Instruction Store_s
        | "add" -> Instruction Add
        | "sub" -> Instruction Sub
        | "mul" -> Instruction Mul
        | "div" -> Instruction Div
        | "jump" -> Instruction Jump
        | "jump.z" -> Instruction Jump_z
        | "jump.nz" -> Instruction Jump_nz
        | "halt" -> Instruction Halt
        | "r0" | "a" -> Register 0x00
        | "r1" | "b" -> Register 0x01
        | "r2" | "c" -> Register 0x02
        | "r3" | "d" -> Register 0x03
        | "r4" | "e" -> Register 0x04
        | "r5" | "f" -> Register 0x05
        | "r6" | "g" -> Register 0x06
        | "r7" | "h" -> Register 0x07
        | "r8" -> Register 0x08
        | "r9" -> Register 0x09
        | "r10" -> Register 0x0A
        | "r11" -> Register 0x0B
        | "r12" -> Register 0x0C
        | "r13" | "fl" -> Register 0x0D
        | "r14" | "sp" -> Register 0x0E
        | "r15" | "pc" -> Register 0x0F
        | Prefix "0x" rest -> Value (Convert.ToInt32(rest, 16))
        | Prefix "0b" rest -> Value (Convert.ToInt32(rest, 2))
        | Prefix "0o" rest -> Value (Convert.ToInt32(rest, 8))
        | w when String.forall (fun c -> Char.IsDigit c) w ->
            Value <| Convert.ToInt32 w
        | t -> failwithf "Unexpected token: %s" t
    
    let tokenizeLine (line: string) =
        let mutable tokens = List.empty<token> in
        line.Split ()
        |> Array.filter (fun w -> w.Length <> 0)
        |> Array.map (fun w -> tokenizeWord w |> int_of_token)
    
    let run (input: string list): int list =
        input
        |> List.filter (fun l -> l.Length > 0)
        |> List.map tokenizeLine
        |> List.collect (fun line -> List.ofArray line) 
