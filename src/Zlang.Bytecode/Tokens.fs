namespace Zlang.Bytecode

module Tokens =
    type token =
    | Instruction of string
    | Value of int
    | Register of int
    | Address of int
    | Unknown of int
    | EOF