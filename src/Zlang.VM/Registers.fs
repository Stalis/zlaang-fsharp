namespace Zlang.VM

module Registers =
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

    let init =
        {r0=0;r1=0;r2=0;r3=0;r4=0;r5=0;r6=0;r7=0;r8=0;r9=0;r10=0;r11=0;r12=0;r13=0;r14=0;r15=0}

    type result =
        | Success
        | Halt
        | Error of string

    let get_register source id =
        match id with
        | R0 | A -> source.r0
        | R1 | B -> source.r1
        | R2 | C -> source.r2
        | R3 | D -> source.r3
        | R4 | E -> source.r4
        | R5 | F -> source.r5
        | R6 | G -> source.r6
        | R7 | H -> source.r7
        | R8 -> source.r8
        | R9 -> source.r9
        | R10 -> source.r10
        | R11 -> source.r11
        | R12 -> source.r12
        | R13 | FL -> source.r13
        | R14 | SP -> source.r14
        | R15 | PC -> source.r15

    let set_register source id v =
        match id with
        | R0 | A -> source.r0 <- v
        | R1 | B -> source.r1 <- v
        | R2 | C -> source.r2 <- v
        | R3 | D -> source.r3 <- v
        | R4 | E -> source.r4 <- v
        | R5 | F -> source.r5 <- v
        | R6 | G -> source.r6 <- v
        | R7 | H -> source.r7 <- v
        | R8 -> source.r8 <- v
        | R9 -> source.r9 <- v
        | R10 -> source.r10 <- v
        | R11 -> source.r11 <- v
        | R12 -> source.r12 <- v
        | R13 | FL -> source.r13 <- v
        | R14 | SP -> source.r14 <- v
        | R15 | PC -> source.r15 <- v
    ()

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
