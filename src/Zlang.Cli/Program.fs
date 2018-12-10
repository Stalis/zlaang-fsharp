// Learn more about F# at http://fsharp.org
open System

open Zlang.Bytecode.Opcodes
open Zlang.Bytecode.Instructions
open Zlang.VM.Cpu
open System.Xml

let bin_prog = [
    0x00; // NOP
    0x10; // Loadc
    0x04; // 4
    0x02; // Dup
    0x32; // Mul
    0xFF; // Halt
]

[<EntryPoint>]
let main argv =
    let vm = VirtualMachine() in
    vm.load_program bin_prog 0
    vm.run

    0 // return an integer exit code
