// Learn more about F# at http://fsharp.org
open System

open Zlang.Bytecode.Opcodes
open Zlang.VM.Cpu
open System.Xml

let bin_prog = [
    0x00; // 0 NOP
    0x10; // 1 Load.c
    0x04; // 2 4
    0x02; // 3 Dup
    0x42; // 4 Jump.nz
    0x08; // 5 8

    0x32; // 6 Mul
    0xFF; // 7 Halt
    0x10; // 8 Load.c
    0x00; // 9 0

    0x40; // A Jump
    0x0F; // B F

    0x00; // C NOP
    0x10; // D Load.c
    0x09; // E 9

    0x20; // F Store.c
    0x00; // 10 R0
    0x10; // 11 0x11

    0x11; // 12 Load.r
    0x00; // 13 0

    0x02; // 14 Dup
    0x32; // 15 Mul
    0x21; // 16 Store.s
    0x01; // 17 R1

    0x11; // 18 Load.r
    0x01; // 19 R2

    0x40; // F Jump
    0x07; // 10 7

]

[<EntryPoint>]
let main argv =
    let vm = VirtualMachine() in
    vm.load_program bin_prog 0
    vm.run

    0 // return an integer exit code
