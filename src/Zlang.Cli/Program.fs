// Learn more about F# at http://fsharp.org
open System

open Zlang.Bytecode.Opcodes
open Zlang.VM

let bin_prog = [
(*0x0*)    Nop;                 // Nop
(*0x1*)    Load_c; 0x04;        // Load.c 4
(*0x3*)    Dup;                 // Dup
(*0x4*)    Jump_nz; 0x08;       // Jump.nz 0x08

(*0x6*)    Mul;                 // Mul
(*0x7*)    Halt;                // Halt
(*0x8*)    Load_c; 0x00;        // Load.c 0

(*0xA*)    Jump; 0x0F;          // Jump 15

(*0xC*)    Nop;                 // Nop
(*0xD*)    Load_c; 0x09;        // Load.c 9

(*0xF*)    Store_c; 0x00; 0x10; // Store.c R0 16

(*0x12*)   Load_r; 0x00;        // Load.r R0

(*0x14*)   Dup;                 // Dup
(*0x15*)   Mul;                 // Mul
(*0x16*)   Store_s; 0x01;       // Store.s R1

(*0x18*)   Load_r; 0x01;        // Load_r R1

(*0x1A*)   Jump; 0x07;          // Jump 7
]

[<EntryPoint>]
let main argv =
    let vm = Zvm.init 128 in
    Zvm.load_dump bin_prog 0 vm;
    Zvm.run vm;

    0 // return an integer exit code
