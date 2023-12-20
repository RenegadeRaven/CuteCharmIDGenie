Public Class AR
    Public Data(&H18) As Byte
    Public Lead(&H90) As Byte
    Public EndCode(&H8) As Byte

    Public Const ButtonCheck As UInteger = &H94000130UI
    Public Const EK4_Length As Byte = &H88

    'will be {ENG, FRE, SPA, ITA, GER, KOR, JAP, CHS, CHT}
    'currently {ENG, FRE, GER}
    Public Shared ReadOnly Pointer_DP As UInteger() = {&HB2106FC0UI, &H2107140UI, &H2107100UI}
    Public Shared ReadOnly Pointer_Pt As UInteger() = {&HB2101D40UI, &H2101F20UI, &H2101EE0UI}
    Public Shared ReadOnly Pointer_HGSS As UInteger() = {&HB2111880UI, &HB21118A0UI, &HB2111860UI}
    Public Const ID_Location_DP As UShort = &H288
    Public Const ID_Location_Pt As UShort = &H8C
    Public Const ID_Location_HGSS As UShort = &H84
    Public Const Box_Location_DP As UShort = &HC370
    Public Const Box_Location_Pt As UShort = &HCF44
    Public Const Box_Location_HGSS As UShort = &HF710
    Public Const Box_Buffer_DPPt As UShort = &HFF0
    Public Const Box_Buffer_HGSS As UShort = &H1000

    Sub New()
        ButtonCondition = ButtonCheck
        ActivateButtons = &HFFFF
        End_CodeType = &HD2
        Lead_CodeType = &HE0
        Lead_Count = EK4_Length
    End Sub
    Public Function Build(IsLead As Boolean)
        Dim Code(&H20) As Byte
        Dim LCode(&HB0) As Byte
        Select Case IsLead
            Case True
                Data.CopyTo(LCode, &H0)
                Lead.CopyTo(LCode, &H18)
                EndCode.CopyTo(LCode, &HA8)
                Return LCode
            Case Else
                Data.CopyTo(Code, &H0)
                EndCode.CopyTo(Code, &H18)
                Return Code
        End Select
    End Function
    Public Property ButtonCondition As UInteger
        Get
            Return BitConverter.ToUInt32(Data.Skip(&H0).Take(4).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UInteger)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H0)
        End Set
    End Property
    Public Property ActivateButtons As UShort
        Get
            Return BitConverter.ToUInt16(Data.Skip(&H4).Take(2).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UShort)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H4)
        End Set
    End Property
#Region "Buttons"
    Public Property Button_A As Boolean
        Get
            Return (ActivateButtons And (1 << 0)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 0)) Or If(value, 0, 1 << 0))
        End Set
    End Property
    Public Property Button_B As Boolean
        Get
            Return (ActivateButtons And (1 << 1)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 1)) Or If(value, 0, 1 << 1))
        End Set
    End Property
    Public Property Button_Select As Boolean
        Get
            Return (ActivateButtons And (1 << 2)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 2)) Or If(value, 0, 1 << 2))
        End Set
    End Property
    Public Property Button_Start As Boolean
        Get
            Return (ActivateButtons And (1 << 3)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 3)) Or If(value, 0, 1 << 3))
        End Set
    End Property
    Public Property Button_Left As Boolean
        Get
            Return (ActivateButtons And (1 << 4)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 4)) Or If(value, 0, 1 << 4))
        End Set
    End Property
    Public Property Button_Right As Boolean
        Get
            Return (ActivateButtons And (1 << 5)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 5)) Or If(value, 0, 1 << 5))
        End Set
    End Property
    Public Property Button_Up As Boolean
        Get
            Return (ActivateButtons And (1 << 6)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 6)) Or If(value, 0, 1 << 6))
        End Set
    End Property
    Public Property Button_Down As Boolean
        Get
            Return (ActivateButtons And (1 << 7)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 7)) Or If(value, 0, 1 << 7))
        End Set
    End Property
    Public Property Button_R As Boolean
        Get
            Return (ActivateButtons And (1 << 8)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 8)) Or If(value, 0, 1 << 8))
        End Set
    End Property
    Public Property Button_L As Boolean
        Get
            Return (ActivateButtons And (1 << 9)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 9)) Or If(value, 0, 1 << 9))
        End Set
    End Property
    Public Property Button_X As Boolean
        Get
            Return (ActivateButtons And (1 << 10)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 10)) Or If(value, 0, 1 << 10))
        End Set
    End Property
    Public Property Button_Y As Boolean
        Get
            Return (ActivateButtons And (1 << 11)) = 0
        End Get
        Set(value As Boolean)
            ActivateButtons = CUShort((ActivateButtons And Not (1 << 11)) Or If(value, 0, 1 << 11))
        End Set
    End Property
#End Region
    Public Property Pointer As UInteger
        Get
            Return BitConverter.ToUInt32(Data.Skip(&H8).Take(4).ToArray().Reverse.ToArray(), 0)
        End Get
        Set(value As UInteger)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H8)
        End Set
    End Property
    Public Property ID_Location As UShort
        Get
            Return BitConverter.ToUInt16(Data.Skip(&H12).Take(2).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UShort)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H12)
        End Set
    End Property
    Public Property SecretID As UShort
        Get
            Return BitConverter.ToUInt16(Data.Skip(&H14).Take(2).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UShort)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H14)
        End Set
    End Property
    Public Property TrainerID As UShort
        Get
            Return BitConverter.ToUInt16(Data.Skip(&H16).Take(2).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UShort)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Data, &H16)
        End Set
    End Property
    Public Property End_CodeType As Byte
        Get
            Return EndCode(&H0)
        End Get
        Set(value As Byte)
            EndCode(&H0) = value
        End Set
    End Property
    Public Property Lead_CodeType As Byte
        Get
            Return Lead(&H0)
        End Get
        Set(value As Byte)
            Lead(&H0) = value
        End Set
    End Property
    Public Property Box_Location As UInteger
        Get
            Return BitConverter.ToUInt32(Lead.Skip(&H0).Take(4).ToArray().Reverse.ToArray(), &H0)
        End Get
        Set(value As UInteger)
            BitConverter.GetBytes(value).Reverse.ToArray().CopyTo(Lead, &H0)
            Lead_CodeType = &HE0
        End Set
    End Property
    Public Property Lead_Count As Byte
        Get
            Return Lead(&H7)
        End Get
        Set(value As Byte)
            Lead(&H7) = value
        End Set
    End Property
    Public Property Lead_EK4 As Byte()
        Get
            Return Lead.Skip(&H8).Take(EK4_Length)
        End Get
        Set(value As Byte())
            value.CopyTo(Lead, &H8)
        End Set
    End Property
End Class