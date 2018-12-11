// Learn more about F# at http://fsharp.org
open System

open System
open System.IO
open Zlang.Bytecode
open Zlang.Bytecode.Opcodes
open Zlang.VM

let binProg = [
(*0x0*)    Nop;                 // Nop              0x00
(*0x1*)    Load_c; 0x04;        // Load.c 4         0x10 0x04
(*0x3*)    Dup;                 // Dup              0x02
(*0x4*)    Jump_nz; 0x08;       // Jump.nz 0x08     0x42 0x08

(*0x6*)    Mul;                 // Mul              0x32
(*0x7*)    Halt;                // Halt             0xFF
(*0x8*)    Load_c; 0x00;        // Load.c 0         0x10 0x00

(*0xA*)    Jump; 0x0F;          // Jump 15          0x40 0x0F

(*0xC*)    Nop;                 // Nop              0x00
(*0xD*)    Load_c; 0x09;        // Load.c 9         0x10 0x09

(*0xF*)    Store_c; 0x00; 0x10; // Store.c R0 16    0x20 0x00 0x10

(*0x12*)   Load_r; 0x00;        // Load.r R0        0x11 0x00

(*0x14*)   Dup;                 // Dup              0x02
(*0x15*)   Mul;                 // Mul              0x32
(*0x16*)   Store_s; 0x01;       // Store.s R1       0x21 0x01

(*0x18*)   Load_r; 0x01;        // Load_r R1        0x11 0x01

(*0x1A*)   Jump; 0x07;          // Jump 7           0x40 0x07
]

let rec read_words bytes =
    match bytes with 
    | a :: b :: c :: d :: rest -> 
        ((int)a <<< 24 ||| (int)b <<< 16 ||| (int)c <<< 8 ||| (int)d <<< 0) :: (read_words rest)
    | _ -> []
    
let rec bytes_of_int (i: int) =
    BitConverter.GetBytes(i)
    |> Array.rev
    
let read_binary path =
    File.ReadAllBytes path 
        |> List.ofArray
        |> read_words

let run mem_size source =
    let vm = Zvm.init mem_size in
    vm |> Zvm.load_dump (read_binary source) 0;
    vm |> Zvm.run 

let disasm source dest =
    use f = new IO.StreamWriter(File.OpenWrite (dest)) in
        f.WriteLine (Disassembler.run <| read_binary source)
          
let asm source dest =
    let lines = File.ReadAllLines source |> List.ofArray in
    use f = new BinaryWriter (File.OpenWrite dest) in
    
    lines
        |>Assembler.run
        |> List.map (fun i -> bytes_of_int i) 
        |> List.iter (fun i -> f.Write i )
    
[<EntryPoint>]
let main argv =
    if argv.Length <> 0 then
        match argv.[0] with
        | "run" -> run 128 argv.[1]
        | "disasm" -> disasm argv.[1] argv.[2]
        | "asm" -> asm argv.[1] argv.[2]
        | _ -> printfn "Unknown command"
    0 // return an integer exit code
