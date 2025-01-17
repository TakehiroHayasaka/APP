﻿
Option Strict Off

Option Explicit On

Imports System.Runtime.InteropServices
Imports HALCONXLib




Imports HalconDotNet

Imports System
Imports Microsoft.VisualBasic


Public Class TargetDetect
    ''' <summary>
    ''' CTのLIST
    ''' </summary>
    Public lstCT As List(Of CodedTarget)
    ''' <summary>
    ''' STのLIST
    ''' </summary>
    Public lstST As List(Of SingleTarget)
    Public Image_ID As Integer
    Public All2D As ImagePoints
    Public All2D_ST As ImagePoints
    Public All2D_ST_Trans As ImagePoints
    Public objTargetRegion As HALCONXLib.HUntypedObjectX 'SUURI ADD 20150403

    ''' <summary>
    ''' 初期化
    ''' </summary>
    Public Sub New()
        Image_ID = -1
        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D = New ImagePoints
        All2D_ST = New ImagePoints
        All2D_ST_Trans = New ImagePoints
        objTargetRegion = Nothing

    End Sub

    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub New(ByVal ImageId As Integer)
        Image_ID = ImageId

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D = New ImagePoints
        All2D_ST = New ImagePoints
        All2D_ST_Trans = New ImagePoints
        objTargetRegion = Nothing

    End Sub
    ''' <summary>
    ''' TargetDetectクラスのlstCTのCT測点情報を、CodedTargetクラスのリストへ書き込む。また、TargetDetectクラスのlstSTのST測点情報を、SingleTargetクラスのリストへ書き込む。
    ''' </summary>
    ''' 



    Public Sub HDevelopStop()
    End Sub

    ' Procedures 
    ' External procedures 
    ' Chapter: Graphics / Text
    ' Short Description: This procedure displays 'Click 'Run' to continue' in the lower right corner of the screen. 
    Public Sub disp_continue_message(ByVal hv_WindowHandle As HTuple, ByVal hv_Color As HTuple, _
        ByVal hv_Box As HTuple)


        ' Local control variables 
        Dim hv_ContinueMessage As HTuple = New HTuple
        Dim hv_Row As HTuple = New HTuple, hv_Column As HTuple = New HTuple
        Dim hv_Width As HTuple = New HTuple, hv_Height As HTuple = New HTuple
        Dim hv_Ascent As HTuple = New HTuple, hv_Descent As HTuple = New HTuple
        Dim hv_TextWidth As HTuple = New HTuple, hv_TextHeight As HTuple = New HTuple

        ' Initialize local and output iconic variables 

        'This procedure displays 'Press Run (F5) to continue' in the
        'lower right corner of the screen.
        'It uses the procedure disp_message.
        '
        'Input parameters:
        'WindowHandle: The window, where the text shall be displayed
        'Color: defines the text color.
        '   If set to '' or 'auto', the currently set color is used.
        'Box: If set to 'true', the text is displayed in a box.
        '
        hv_ContinueMessage = New HTuple("Press Run (F5) to continue")
        HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_Row, hv_Column, hv_Width, hv_Height)
        HOperatorSet.GetStringExtents(hv_WindowHandle, (((New HTuple(" ")).TupleAdd(hv_ContinueMessage))).TupleAdd( _
            New HTuple(" ")), hv_Ascent, hv_Descent, hv_TextWidth, hv_TextHeight)
        disp_message(hv_WindowHandle, hv_ContinueMessage, New HTuple("window"), ((hv_Height.TupleSub( _
            hv_TextHeight))).TupleSub(New HTuple(12)), ((hv_Width.TupleSub(hv_TextWidth))).TupleSub( _
            New HTuple(12)), hv_Color, hv_Box)

        Exit Sub
    End Sub

    ' Chapter: Graphics / Text
    ' Short Description: This procedure writes a text message. 
    Public Sub disp_message(ByVal hv_WindowHandle As HTuple, ByVal hv_String As HTuple, _
        ByVal hv_CoordSystem As HTuple, ByVal hv_Row As HTuple, ByVal hv_Column As HTuple, _
        ByVal hv_Color As HTuple, ByVal hv_Box As HTuple)


        ' Local control variables 
        Dim hv_Red As HTuple = New HTuple, hv_Green As HTuple = New HTuple
        Dim hv_Blue As HTuple = New HTuple, hv_Row1Part As HTuple = New HTuple
        Dim hv_Column1Part As HTuple = New HTuple, hv_Row2Part As HTuple = New HTuple
        Dim hv_Column2Part As HTuple = New HTuple, hv_RowWin As HTuple = New HTuple
        Dim hv_ColumnWin As HTuple = New HTuple, hv_WidthWin As HTuple = New HTuple
        Dim hv_HeightWin As HTuple = New HTuple, hv_MaxAscent As HTuple = New HTuple
        Dim hv_MaxDescent As HTuple = New HTuple, hv_MaxWidth As HTuple = New HTuple
        Dim hv_MaxHeight As HTuple = New HTuple, hv_R1 As HTuple = New HTuple
        Dim hv_C1 As HTuple = New HTuple, hv_FactorRow As HTuple = New HTuple
        Dim hv_FactorColumn As HTuple = New HTuple, hv_Width As HTuple = New HTuple
        Dim hv_Index As HTuple = New HTuple, hv_Ascent As HTuple = New HTuple
        Dim hv_Descent As HTuple = New HTuple, hv_W As HTuple = New HTuple
        Dim hv_H As HTuple = New HTuple, hv_FrameHeight As HTuple = New HTuple
        Dim hv_FrameWidth As HTuple = New HTuple, hv_R2 As HTuple = New HTuple
        Dim hv_C2 As HTuple = New HTuple, hv_DrawMode As HTuple = New HTuple
        Dim hv_Exception As HTuple = New HTuple, hv_CurrentColor As HTuple = New HTuple

        Dim hv_Color_COPY_INP_TMP As HTuple
        hv_Color_COPY_INP_TMP = hv_Color.Clone()
        Dim hv_Column_COPY_INP_TMP As HTuple
        hv_Column_COPY_INP_TMP = hv_Column.Clone()
        Dim hv_Row_COPY_INP_TMP As HTuple
        hv_Row_COPY_INP_TMP = hv_Row.Clone()
        Dim hv_String_COPY_INP_TMP As HTuple
        hv_String_COPY_INP_TMP = hv_String.Clone()

        ' Initialize local and output iconic variables 

        'This procedure displays text in a graphics window.
        '
        'Input parameters:
        'WindowHandle: The WindowHandle of the graphics window, where
        '   the message should be displayed
        'String: A tuple of strings containing the text message to be displayed
        'CoordSystem: If set to 'window', the text position is given
        '   with respect to the window coordinate system.
        '   If set to 'image', image coordinates are used.
        '   (This may be useful in zoomed images.)
        'Row: The row coordinate of the desired text position
        '   If set to -1, a default value of 12 is used.
        'Column: The column coordinate of the desired text position
        '   If set to -1, a default value of 12 is used.
        'Color: defines the color of the text as string.
        '   If set to [], '' or 'auto' the currently set color is used.
        '   If a tuple of strings is passed, the colors are used cyclically
        '   for each new textline.
        'Box: If set to 'true', the text is written within a white box.
        '
        'prepare window
        HOperatorSet.GetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
        HOperatorSet.GetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
            hv_Column2Part)
        HOperatorSet.GetWindowExtents(hv_WindowHandle, hv_RowWin, hv_ColumnWin, hv_WidthWin, _
            hv_HeightWin)
        HOperatorSet.SetPart(hv_WindowHandle, New HTuple(0), New HTuple(0), hv_HeightWin.TupleSub( _
            New HTuple(1)), hv_WidthWin.TupleSub(New HTuple(1)))
        '
        'default settings
        If New HTuple(hv_Row_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
            hv_Row_COPY_INP_TMP = New HTuple(12)
        End If
        If New HTuple(hv_Column_COPY_INP_TMP.TupleEqual(New HTuple(-1))).I() Then
            hv_Column_COPY_INP_TMP = New HTuple(12)
        End If
        If New HTuple(hv_Color_COPY_INP_TMP.TupleEqual(New HTuple())).I() Then
            hv_Color_COPY_INP_TMP = New HTuple("")
        End If
        '
        hv_String_COPY_INP_TMP = (((((New HTuple("")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
            New HTuple("")))).TupleSplit(New HTuple("" + Chr(10)))
        '
        'Estimate extentions of text depending on font size.
        HOperatorSet.GetFontExtents(hv_WindowHandle, hv_MaxAscent, hv_MaxDescent, hv_MaxWidth, _
            hv_MaxHeight)
        If New HTuple(hv_CoordSystem.TupleEqual(New HTuple("window"))).I() Then
            hv_R1 = hv_Row_COPY_INP_TMP.Clone()
            hv_C1 = hv_Column_COPY_INP_TMP.Clone()
        Else
            'transform image to window coordinates
            hv_FactorRow = (((New HTuple(1.0)).TupleMult(hv_HeightWin))).TupleDiv(((hv_Row2Part.TupleSub( _
                hv_Row1Part))).TupleAdd(New HTuple(1)))
            hv_FactorColumn = (((New HTuple(1.0)).TupleMult(hv_WidthWin))).TupleDiv(((hv_Column2Part.TupleSub( _
                hv_Column1Part))).TupleAdd(New HTuple(1)))
            hv_R1 = ((((hv_Row_COPY_INP_TMP.TupleSub(hv_Row1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                hv_FactorRow)
            hv_C1 = ((((hv_Column_COPY_INP_TMP.TupleSub(hv_Column1Part))).TupleAdd(New HTuple(0.5)))).TupleMult( _
                hv_FactorColumn)
        End If
        '
        'display text box depending on text size
        If New HTuple(hv_Box.TupleEqual(New HTuple("true"))).I() Then
            'calculate box extents
            hv_String_COPY_INP_TMP = (((New HTuple(" ")).TupleAdd(hv_String_COPY_INP_TMP))).TupleAdd( _
                New HTuple(" "))
            hv_Width = New HTuple()
            For hv_Index = (New HTuple(0)) To ((New HTuple( _
                hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                    hv_Index), hv_Ascent, hv_Descent, hv_W, hv_H)
                hv_Width = hv_Width.TupleConcat(hv_W)
#If USE_DO_EVENTS Then
        ' Please note: The call of DoEvents() is only a hack to
        ' enable VB to react on events. Please change the code
        ' so that it can handle events in a standard way.
        System.Windows.Forms.Application.DoEvents()
#End If
            Next
            hv_FrameHeight = hv_MaxHeight.TupleMult(New HTuple(hv_String_COPY_INP_TMP.TupleLength( _
                )))
            hv_FrameWidth = (((New HTuple(0)).TupleConcat(hv_Width))).TupleMax()
            hv_R2 = hv_R1.TupleAdd(hv_FrameHeight)
            hv_C2 = hv_C1.TupleAdd(hv_FrameWidth)
            'display rectangles
            HOperatorSet.GetDraw(hv_WindowHandle, hv_DrawMode)
            HOperatorSet.SetDraw(hv_WindowHandle, New HTuple("fill"))
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("light gray"))
            HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1.TupleAdd(New HTuple(3)), _
                hv_C1.TupleAdd(New HTuple(3)), hv_R2.TupleAdd(New HTuple(3)), hv_C2.TupleAdd( _
                New HTuple(3)))
            HOperatorSet.SetColor(hv_WindowHandle, New HTuple("white"))
            HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2)
            HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode)
        ElseIf New HTuple(hv_Box.TupleNotEqual(New HTuple("false"))).I() Then
            hv_Exception = New HTuple("Wrong value of control parameter Box")
            Throw New HalconException(hv_Exception)
        End If
        'Write text.
        For hv_Index = (New HTuple(0)) To ( _
            (New HTuple(hv_String_COPY_INP_TMP.TupleLength()))).TupleSub(New HTuple(1)) Step (New HTuple(1))
            hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index.TupleMod(New HTuple( _
                hv_Color_COPY_INP_TMP.TupleLength())))
            If ((New HTuple(hv_CurrentColor.TupleNotEqual(New HTuple(""))))).TupleAnd(New HTuple( _
                hv_CurrentColor.TupleNotEqual(New HTuple("auto")))).I() Then
                HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor)
            Else
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
            End If
            hv_Row_COPY_INP_TMP = hv_R1.TupleAdd(hv_MaxHeight.TupleMult(hv_Index))
            HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1)
            HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect( _
                hv_Index))
#If USE_DO_EVENTS Then
      ' Please note: The call of DoEvents() is only a hack to
      ' enable VB to react on events. Please change the code
      ' so that it can handle events in a standard way.
      System.Windows.Forms.Application.DoEvents()
#End If
        Next
        'reset changed window settings
        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue)
        HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part, _
            hv_Column2Part)

        Exit Sub
    End Sub

    ' Chapter: Graphics / Text
    ' Short Description: Set font independent of OS 
    Public Sub set_display_font(ByVal hv_WindowHandle As HTuple, ByVal hv_Size As HTuple, _
        ByVal hv_Font As HTuple, ByVal hv_Bold As HTuple, ByVal hv_Slant As HTuple)


        ' Local control variables 
        Dim hv_OS As HTuple = New HTuple, hv_Exception As HTuple = New HTuple
        Dim hv_BoldString As HTuple = New HTuple, hv_SlantString As HTuple = New HTuple
        Dim hv_AllowedFontSizes As HTuple = New HTuple, hv_Distances As HTuple = New HTuple
        Dim hv_Indices As HTuple = New HTuple, hv_Fonts As HTuple = New HTuple
        Dim hv_FontSelRegexp As HTuple = New HTuple, hv_FontsCourier As HTuple = New HTuple

        Dim hv_Bold_COPY_INP_TMP As HTuple
        hv_Bold_COPY_INP_TMP = hv_Bold.Clone()
        Dim hv_Font_COPY_INP_TMP As HTuple
        hv_Font_COPY_INP_TMP = hv_Font.Clone()
        Dim hv_Size_COPY_INP_TMP As HTuple
        hv_Size_COPY_INP_TMP = hv_Size.Clone()
        Dim hv_Slant_COPY_INP_TMP As HTuple
        hv_Slant_COPY_INP_TMP = hv_Slant.Clone()

        ' Initialize local and output iconic variables 

        'This procedure sets the text font of the current window with
        'the specified attributes.
        'It is assumed that following fonts are installed on the system:
        'Windows: Courier New, Arial Times New Roman
        'Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
        'Linux: courier, helvetica, times
        'Because fonts are displayed smaller on Linux than on Windows,
        'a scaling factor of 1.25 is used the get comparable results.
        'For Linux, only a limited number of font sizes is supported,
        'to get comparable results, it is recommended to use one of the
        'following sizes: 9, 11, 14, 16, 20, 27
        '(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
        '
        'Input parameters:
        'WindowHandle: The graphics window for which the font will be set
        'Size: The font size. If Size=-1, the default of 16 is used.
        'Bold: If set to 'true', a bold font is used
        'Slant: If set to 'true', a slanted font is used
        '
        HOperatorSet.GetSystem(New HTuple("operating_system"), hv_OS)
        ' dev_get_preferences(...); only in hdevelop
        ' dev_set_preferences(...); only in hdevelop
        If ((New HTuple(hv_Size_COPY_INP_TMP.TupleEqual(New HTuple())))).TupleOr(New HTuple( _
            hv_Size_COPY_INP_TMP.TupleEqual(New HTuple(-1)))).I() Then
            hv_Size_COPY_INP_TMP = New HTuple(16)
        End If
        If New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(2)))).TupleEqual(New HTuple("Win"))).I() Then
            'Set font on Windows systems
            If ((((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Courier New")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Arial")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Times New Roman")
            End If
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple(1)
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple(0)
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple(1)
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple(0)
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            Try
                HOperatorSet.SetFont(hv_WindowHandle, (((((((((((((((New HTuple("-")).TupleAdd( _
                    hv_Font_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Size_COPY_INP_TMP))).TupleAdd( _
                    New HTuple("-*-")))).TupleAdd(hv_Slant_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-")))).TupleAdd( _
                    hv_Bold_COPY_INP_TMP))).TupleAdd(New HTuple("-")))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                'throw (Exception)
            End Try
        ElseIf New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(2)))).TupleEqual( _
            New HTuple("Dar"))).I() Then
            'Set font on Mac OS X systems
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_BoldString = New HTuple("Bold")
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_BoldString = New HTuple("")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_SlantString = New HTuple("Italic")
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_SlantString = New HTuple("")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            If ((((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("CourierNewPS")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("Arial")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("TimesNewRomanPS")
            End If
            If ((New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))))).TupleOr( _
                New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true")))).I() Then
                hv_Font_COPY_INP_TMP = ((((hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("-")))).TupleAdd( _
                    hv_BoldString))).TupleAdd(hv_SlantString)
            End If
            hv_Font_COPY_INP_TMP = hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("MT"))
            Try
                HOperatorSet.SetFont(hv_WindowHandle, ((hv_Font_COPY_INP_TMP.TupleAdd(New HTuple("-")))).TupleAdd( _
                    hv_Size_COPY_INP_TMP))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                'throw (Exception)
            End Try
        Else
            'Set font for UNIX systems
            hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP.TupleMult(New HTuple(1.25))
            hv_AllowedFontSizes = (((((New HTuple(11)).TupleConcat(14)).TupleConcat(17)).TupleConcat( _
                20)).TupleConcat(25)).TupleConcat(34)
            If New HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual( _
                New HTuple(-1))).I() Then
                hv_Distances = ((hv_AllowedFontSizes.TupleSub(hv_Size_COPY_INP_TMP))).TupleAbs( _
                    )
                HOperatorSet.TupleSortIndex(hv_Distances, hv_Indices)
                hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect( _
                    New HTuple(0)))
            End If
            If ((New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("mono"))))).TupleOr( _
                New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("Courier")))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("courier")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("sans"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("helvetica")
            ElseIf New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("serif"))).I() Then
                hv_Font_COPY_INP_TMP = New HTuple("times")
            End If
            If New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple("bold")
            ElseIf New HTuple(hv_Bold_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Bold_COPY_INP_TMP = New HTuple("medium")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Bold")
                Throw New HalconException(hv_Exception)
            End If
            If New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("true"))).I() Then
                If New HTuple(hv_Font_COPY_INP_TMP.TupleEqual(New HTuple("times"))).I() Then
                    hv_Slant_COPY_INP_TMP = New HTuple("i")
                Else
                    hv_Slant_COPY_INP_TMP = New HTuple("o")
                End If
            ElseIf New HTuple(hv_Slant_COPY_INP_TMP.TupleEqual(New HTuple("false"))).I() Then
                hv_Slant_COPY_INP_TMP = New HTuple("r")
            Else
                hv_Exception = New HTuple("Wrong value of control parameter Slant")
                Throw New HalconException(hv_Exception)
            End If
            Try
                HOperatorSet.SetFont(hv_WindowHandle, (((((((((((((((New HTuple("-adobe-")).TupleAdd( _
                    hv_Font_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Bold_COPY_INP_TMP))).TupleAdd( _
                    New HTuple("-")))).TupleAdd(hv_Slant_COPY_INP_TMP))).TupleAdd(New HTuple("-normal-*-")))).TupleAdd( _
                    hv_Size_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-*-*-*-*-*")))
                ' catch (Exception) 
            Catch HDevExpDefaultException1 As HalconException
                HDevExpDefaultException1.ToHTuple(hv_Exception)
                If ((New HTuple(((hv_OS.TupleSubstr(New HTuple(0), New HTuple(4)))).TupleEqual( _
                    New HTuple("Linux"))))).TupleAnd(New HTuple(hv_Font_COPY_INP_TMP.TupleEqual( _
                    New HTuple("courier")))).I() Then
                    HOperatorSet.QueryFont(hv_WindowHandle, hv_Fonts)
                    hv_FontSelRegexp = (((((New HTuple("^-[^-]*-[^-]*[Cc]ourier[^-]*-")).TupleAdd( _
                        hv_Bold_COPY_INP_TMP))).TupleAdd(New HTuple("-")))).TupleAdd(hv_Slant_COPY_INP_TMP)
                    hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch( _
                        hv_FontSelRegexp)
                    If New HTuple(((New HTuple(hv_FontsCourier.TupleLength()))).TupleEqual( _
                        New HTuple(0))).I() Then
                        hv_Exception = New HTuple("Wrong font name")
                        'throw (Exception)
                    Else
                        Try
                            HOperatorSet.SetFont(hv_WindowHandle, ((((((hv_FontsCourier.TupleSelect( _
                                New HTuple(0)))).TupleAdd(New HTuple("-normal-*-")))).TupleAdd( _
                                hv_Size_COPY_INP_TMP))).TupleAdd(New HTuple("-*-*-*-*-*-*-*")))
                            ' catch (Exception) 
                        Catch HDevExpDefaultException2 As HalconException
                            HDevExpDefaultException2.ToHTuple(hv_Exception)
                            'throw (Exception)
                        End Try
                    End If
                End If
                'throw (Exception)
            End Try
        End If
        ' dev_set_preferences(...); only in hdevelop

        Exit Sub
    End Sub






    Public Sub SaveData()
        'Dim strSaveFullPath As String
        'strSaveFullPath = strSavePath & "Targets" & "\"
        'Dim folderExists As Boolean
        'folderExists = My.Computer.FileSystem.DirectoryExists(strSaveFullPath)
        'If folderExists = False Then
        '    My.Computer.FileSystem.CreateDirectory(strSaveFullPath)
        'End If

        'Dim objSaveTuple As Object = Nothing
        'ExtendVar(objSaveTuple, 2)
        'objSaveTuple.setvalue(Image_ID, 0)
        'objSaveTuple.setvalue(lstCT.Count, 1)
        'objSaveTuple.setvalue(lstST.Count, 2)
        'SaveTupleObj(objSaveTuple, strSaveFullPath & "Common.tpl")
        'Dim CT_IDs As Object = DBNull.Value
        For Each CT As CodedTarget In lstCT
            CT.SaveData()
            '  CT_IDs = Tuple.TupleConcat(CT_IDs, CT.CT_ID)
        Next
        '  SaveTupleObj(CT_IDs, strSaveFullPath & "CT_IDs.tpl")
        '   Dim ST_IDs As Object = DBNull.Value


        'For Each ST As SingleTarget In lstST
        '    ST.SaveData()
        '    '   ST_IDs = Tuple.TupleConcat(ST_IDs, ST.P2ID)
        'Next


        'SaveTupleObj(ST_IDs, strSaveFullPath & "ST_IDs.tpl")

        'All2D.SaveData(strSaveFullPath & "All2D")
        'All2D_ST.SaveData(strSaveFullPath & "All2D_ST")

    End Sub
    Public Sub SaveDataCTonly()

        For Each CT As CodedTarget In lstCT
            CT.SaveDataAllTargets()
        Next
    End Sub
    '20150130 ADD By Yamada sta--------------------------------------------------- 
    ''' <summary>
    ''' Targetsテーブルの重複したレコードを削除する
    ''' </summary>
    Public Sub DeleteData(ByVal strImageID As String, ByVal blnNot As Boolean)
        Dim strWhere As String = ""

        If strImageID <> "" Then
            If blnNot = True Then
                strWhere = "NOT ImageID=" & strImageID
            Else
                strWhere = "ImageID=" & strImageID
            End If
        End If
        

        If dbClass.DoDelete("Targets", strWhere) < 0 Then
            'MsgBox("DB更新に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub
    'SUURI ADD START 20150410
    Public Sub DeleteDataAllTarget(ByVal strImageID As String, ByVal blnNot As Boolean)
        Dim strWhere As String = ""

        If strImageID <> "" Then
            If blnNot = True Then
                strWhere = "NOT ImageID=" & strImageID
            Else
                strWhere = "ImageID=" & strImageID
            End If
        End If


        If dbClass.DoDelete("AllTargets", strWhere) < 0 Then
            'MsgBox("DB更新に失敗しました。", MsgBoxStyle.OkOnly, "エラー")
            Exit Sub
        End If
    End Sub
    'SUURI ADD END 20150410

    '20150130 ADD By Yamada end---------------------------------------------------

    ''' <summary>
    ''' 画像のパスと画像IDを読み込む
    ''' </summary>
    Public Sub ReadData(ByVal strReadPath As String, ByVal ImgID As Integer)
        'Dim strReadFullPath As String
        'strReadFullPath = strReadPath & "Targets" & "\"
        'Dim folderExists As Boolean
        'folderExists = My.Computer.FileSystem.DirectoryExists(strReadFullPath)
        'If folderExists = False Then
        '    Exit Sub
        'End If
        Dim i As Integer
        'Dim lstCTnum As Integer
        'Dim lstSTnum As Integer

        Dim IDR As IDataReader
        Dim strSqlText As String = ""

        'Dim objOtherParams As New Object
        'If ReadTupleObj(objOtherParams, strReadFullPath & "Common.tpl") Then
        '    Image_ID = CInt(Tuple.TupleSelect(objOtherParams, 0))
        '    lstCTnum = CInt(Tuple.TupleSelect(objOtherParams, 1))
        '    lstSTnum = CInt(Tuple.TupleSelect(objOtherParams, 2))
        'End If
        'Dim CT_IDs As Object = Nothing
        'ReadTupleObj(CT_IDs, strReadFullPath & "CT_IDs.tpl")
        'For i = 0 To lstCTnum - 1
        '    Dim CT As New CodedTarget
        '    CT.CT_ID = CInt(Tuple.TupleSelect(CT_IDs, i))
        '    CT.ReadData(strReadFullPath)
        '    lstCT.Add(CT)
        'Next
        All2D.Row = DBNull.Value
        All2D.Col = DBNull.Value

        strSqlText = "SELECT "
        Dim n As Integer = SingleTargetFields.Length
        For i = 0 To n - 2
            strSqlText = strSqlText & SingleTargetFields(i) & ","
        Next
        strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
        strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=2 ORDER BY P2ID,P3ID"
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim CT As New CodedTarget
                CT.ReadData(IDR)
                lstCT.Add(CT)
#If Halcon = 11 Then
                All2D.Row = Tuple.TupleConcat(All2D.Row, CT.CT_Points.Row)
                All2D.Col = Tuple.TupleConcat(All2D.Col, CT.CT_Points.Col)
#End If
            Loop
            IDR.Close()
        End If



        'Dim ST_IDs As Object = Nothing
        'ReadTupleObj(ST_IDs, strReadFullPath & "ST_IDs.tpl")
        'For i = 0 To lstSTnum - 1
        '    Dim ST As New SingleTarget
        '    ST.P2ID = CInt(Tuple.TupleSelect(ST_IDs, i))
        '    ST.ReadData(strReadFullPath)
        '    lstST.Add(ST)
        'Next

        All2D_ST.Row = DBNull.Value
        All2D_ST.Col = DBNull.Value
        strSqlText = "SELECT "
        For i = 0 To n - 2
            strSqlText = strSqlText & SingleTargetFields(i) & ","
        Next
        strSqlText = strSqlText & SingleTargetFields(n - 1) & " "
        strSqlText = strSqlText & "FROM Targets WHERE ImageID=" & ImgID & " AND flgType=1 ORDER BY P2ID"
        IDR = dbClass.DoSelect(strSqlText)
        If Not IDR Is Nothing Then
            Do While IDR.Read
                Dim ST As New SingleTarget
                ST.ReadData(IDR)
                lstST.Add(ST)
#If Halcon = 11 Then
                All2D.Row = Tuple.TupleConcat(All2D.Row, ST.P2D.Row)
                All2D.Col = Tuple.TupleConcat(All2D.Col, ST.P2D.Col)
                All2D_ST.Row = Tuple.TupleConcat(All2D_ST.Row, ST.P2D.Row)
                All2D_ST.Col = Tuple.TupleConcat(All2D_ST.Col, ST.P2D.Col)
#End If
            Loop
            IDR.Close()
        End If

        'All2D.ReadData(strReadFullPath & "All2D")
        'All2D_ST.ReadData(strReadFullPath & "All2D_ST")

    End Sub
    ''' <summary>
    ''' 未使用
    ''' </summary>
    Public Sub DetectCT_old(ByVal ho_Image As HALCONXLib.HUntypedObjectX, ByVal hv_CT_ALL_ID As Object, ByVal T_Threshold As Integer)
        ' Stack for temporary objects 
        Dim OTemp(10) As HUntypedObjectX
        Dim SP_O As Integer
        SP_O = 0

        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Local iconic variables 
        Dim ho_Region As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions As HUntypedObjectX = Nothing
        Dim ho_RegionFillUp As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection2 As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected1 As HUntypedObjectX = Nothing
        Dim ho_RegionErosion As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection3 As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions4 As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection As HUntypedObjectX = Nothing
        Dim ho_InnerTargets As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions5 As HUntypedObjectX = Nothing
        Dim ho_RegionDifference As HUntypedObjectX = Nothing
        Dim ho_OuterTargets As HUntypedObjectX = Nothing
        Dim ho_tempObj As HUntypedObjectX = Nothing

        ' Local control variables 
        Dim hv_ExpDefaultCtrlDummyVar As Object = Nothing
        Dim hv_NumConnected As Object = Nothing, hv_NumHoles As Object = Nothing
        Dim hv_Number1 As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Cols As Object = Nothing, hv_j As Object = Nothing
        Dim hv_i As Object = Nothing, hv_Row5 As Object = Nothing
        Dim hv_Column3 As Object = Nothing, hv_Phi As Object = Nothing
        Dim hv_Length11 As Object = Nothing, hv_Length21 As Object = Nothing
        Dim hv_Area2 As Object = Nothing, hv_Row2 As Object = Nothing
        Dim hv_Column As Object = Nothing, hv_Mean As Object = Nothing
        Dim hv_Temp As Object = Nothing, hv_Number2 As Object = Nothing
        Dim hv_Area As Object = Nothing, hv_Row As Object = Nothing
        Dim hv_Col As Object = Nothing, hv_Row1 As Object = Nothing
        Dim hv_Col1 As Object = Nothing, hv_Area3 As Object = Nothing
        Dim hv_Mean1 As Object = Nothing, hv_BRow As Object = Nothing
        Dim hv_BCol As Object = Nothing, hv_mindist As Object = Nothing
        Dim hv_tempIndex As Object = Nothing, hv_C0_Row As Object = Nothing
        Dim hv_C0_Col As Object = Nothing, hv_C1_Row As Object = Nothing
        Dim hv_C1_Col As Object = Nothing, hv_C2_Row As Object = Nothing
        Dim hv_C2_Col As Object = Nothing, hv_C3_Row As Object = Nothing
        Dim hv_C3_Col As Object = Nothing, hv_t As Object = Nothing
        Dim hv_Distance As Object = Nothing, hv_blnCheck As Object = Nothing
        Dim hv_Vec1_R As Object = Nothing, hv_Vec1_C As Object = Nothing
        Dim hv_Vec2_R As Object = Nothing, hv_Vec2_C As Object = Nothing
        Dim hv_Vec3_R As Object = Nothing, hv_Vec3_C As Object = Nothing
        Dim hv_Gaiseki1 As Object = Nothing, hv_Gaiseki2 As Object = Nothing
        Dim hv_Result As Object = Nothing, hv_I1 As Object = Nothing
        Dim hv_I2 As Object = Nothing, hv_I3 As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_HomMat2D As Object = Nothing, hv_Covariance As Object = Nothing
        Dim hv_GRow As Object = Nothing, hv_GCol As Object = Nothing
        Dim hv_Angle As Object = Nothing, hv_Quot As Object = Nothing
        Dim hv_Int As Object = Nothing, hv_STR As Object = Nothing
        Dim hv_CTSTRCODE As Object = Nothing, hv_Indices As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InCol As Object = Nothing, hv_AngIndex As Object = Nothing
        'Dim hv_O1_Row As Object = Nothing, hv_O1_Col As Object = Nothing
        'Dim hv_O2_Row As Object = Nothing, hv_O2_Col As Object = Nothing
        'Dim hv_O3_Row As Object = Nothing, hv_O3_Col As Object = Nothing
        'Dim hv_O4_Row As Object = Nothing, hv_O4_Col As Object = Nothing

        ' Initialize local and output iconic variables 

        Op.GenEmptyObj(ho_Region)
        Op.GenEmptyObj(ho_ConnectedRegions)
        Op.GenEmptyObj(ho_SelectedRegions2)
        Op.GenEmptyObj(ho_SelectedRegions)
        Op.GenEmptyObj(ho_RegionFillUp)
        Op.GenEmptyObj(ho_RegionIntersection2)
        Op.GenEmptyObj(ho_ObjectSelected1)
        Op.GenEmptyObj(ho_RegionErosion)
        Op.GenEmptyObj(ho_RegionIntersection3)
        Op.GenEmptyObj(ho_SelectedRegions4)
        Op.GenEmptyObj(ho_RegionIntersection)
        Op.GenEmptyObj(ho_InnerTargets)
        Op.GenEmptyObj(ho_SelectedRegions5)
        Op.GenEmptyObj(ho_RegionDifference)
        Op.GenEmptyObj(ho_OuterTargets)
        Op.GenEmptyObj(ho_tempObj)

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If
        Marshal.ReleaseComObject(ho_Region)
        Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        Op.VarThreshold(ho_Image, ho_Region, 30, 30, 0.2, T_Threshold, "light")

        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Op.Connection(ho_Region, ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.SelectShape(ho_ConnectedRegions, ho_SelectedRegions2, "area", "and", 10, 99999)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Op.SelectShape(ho_SelectedRegions2, ho_SelectedRegions, "holes_num", "and", 1, _
            1)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection2)
        Op.Intersection(ho_RegionFillUp, ho_SelectedRegions2, ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.SelectShape(ho_RegionIntersection2, ho_SelectedRegions2, "connect_num", "and", _
            9, 9)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        Op.CopyObj(ho_RegionFillUp, OTemp(SP_O), 1, -1)
        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.Connection(OTemp(SP_O - 1), ho_RegionFillUp)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0
        Op.CopyObj(ho_SelectedRegions2, OTemp(SP_O), 1, -1)
        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.Connection(OTemp(SP_O - 1), ho_SelectedRegions2)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0
        Op.ConnectAndHoles(ho_RegionIntersection2, hv_NumConnected, hv_NumHoles)
        Op.CountObj(ho_RegionFillUp, hv_Number1)
        hv_Rows = System.DBNull.Value
        hv_Cols = System.DBNull.Value
        hv_j = 0
        For hv_i = 1 To hv_Number1 Step 1
            Marshal.ReleaseComObject(ho_ObjectSelected1)
            Op.SelectObj(ho_RegionFillUp, ho_ObjectSelected1, hv_i)
            Op.SmallestRectangle2(ho_ObjectSelected1, hv_Row5, hv_Column3, hv_Phi, hv_Length11, _
                hv_Length21)
            Marshal.ReleaseComObject(ho_RegionErosion)
            Op.GenEllipse(ho_RegionErosion, hv_Row5, hv_Column3, hv_Phi, Tuple.TupleDiv(hv_Length11, _
                2), Tuple.TupleDiv(hv_Length21, 2))
            Marshal.ReleaseComObject(ho_RegionIntersection3)
            Op.Intersection(ho_SelectedRegions2, ho_ObjectSelected1, ho_RegionIntersection3 _
                )
            Marshal.ReleaseComObject(ho_SelectedRegions4)
            Op.SelectShape(ho_RegionIntersection3, ho_SelectedRegions4, "area", "and", 1, _
                99999)
            Marshal.ReleaseComObject(ho_RegionIntersection)
            Op.Intersection(ho_SelectedRegions4, ho_RegionErosion, ho_RegionIntersection)
            Op.CopyObj(ho_RegionIntersection, OTemp(SP_O), 1, -1)
            SP_O = SP_O + 1
            Marshal.ReleaseComObject(ho_RegionIntersection)
            Op.SelectShape(OTemp(SP_O - 1), ho_RegionIntersection, "area", "and", 1, 99999)
            Marshal.ReleaseComObject(OTemp(SP_O - 1))
            SP_O = 0
            Op.AreaCenter(ho_RegionIntersection, hv_Area2, hv_Row2, hv_Column)
            Op.TupleMean(hv_Area2, hv_Mean)
            If Tuple.TupleLessEqual(Tuple.TupleLength(hv_Area2), 4) = 1 Then
                Marshal.ReleaseComObject(ho_InnerTargets)
                Op.SelectShape(ho_RegionIntersection, ho_InnerTargets, "area", "and", 1, _
                    9999999)
            Else
                Marshal.ReleaseComObject(ho_InnerTargets)
                Op.SelectShape(ho_RegionIntersection, ho_InnerTargets, "area", "and", Tuple.TupleSub( _
                    hv_Mean, 10), 9999999)
            End If
            Op.CountObj(ho_InnerTargets, hv_Number2)
            If Tuple.TupleEqual(hv_Number2, 4) = 1 Then
                Op.AreaCenter(ho_ObjectSelected1, hv_Area, hv_Row, hv_Col)
                Marshal.ReleaseComObject(ho_tempObj)
                Op.DilationCircle(ho_InnerTargets, ho_tempObj, 2)
                Op.AreaCenterGray(ho_tempObj, ho_Image, hv_Area, hv_Row1, hv_Col1)
                '外側のターゲットを抽出
                Op.AreaCenter(ho_SelectedRegions4, hv_Area3, hv_ExpDefaultCtrlDummyVar, hv_ExpDefaultCtrlDummyVar)
                Op.TupleMean(hv_Area3, hv_Mean1)
                Marshal.ReleaseComObject(ho_SelectedRegions5)
                Op.SelectShape(ho_SelectedRegions4, ho_SelectedRegions5, "area", "and", 1, _
                    hv_Mean1)
                Marshal.ReleaseComObject(ho_RegionDifference)
                Op.Difference(ho_SelectedRegions5, ho_InnerTargets, ho_RegionDifference _
                    )
                Marshal.ReleaseComObject(ho_OuterTargets)
                Op.SelectShape(ho_RegionDifference, ho_OuterTargets, "area", "and", 1, 99999)
                Marshal.ReleaseComObject(ho_tempObj)
                Op.DilationCircle(ho_OuterTargets, ho_tempObj, 2)
                Op.AreaCenterGray(ho_tempObj, ho_Image, hv_ExpDefaultCtrlDummyVar, hv_OutRow, hv_OutCol)
                '外側のターゲットを抽出
                hv_mindist = 999999
                hv_tempIndex = -1
                hv_C0_Row = 0
                hv_C0_Col = 0
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                For hv_t = 0 To 3 Step 1
                    Op.DistancePp(hv_Row, hv_Col, Tuple.TupleSelect(hv_Row1, hv_t), Tuple.TupleSelect( _
                        hv_Col1, hv_t), hv_Distance)
                    If Tuple.TupleGreater(hv_mindist, hv_Distance) = 1 Then
                        hv_C0_Row = Tuple.TupleSelect(hv_Row1, hv_t)
                        hv_C0_Col = Tuple.TupleSelect(hv_Col1, hv_t)
                        hv_tempIndex = hv_t
                        hv_mindist = hv_Distance
                    End If
                Next
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                Op.TupleConcat(CTemp(SP_C - 1), hv_C0_Row, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Cols
                SP_C = SP_C + 1
                Op.TupleConcat(CTemp(SP_C - 1), hv_C0_Col, hv_Cols)
                SP_C = 0
                CTemp(SP_C) = hv_Row1
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Row1)
                SP_C = 0
                CTemp(SP_C) = hv_Col1
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Col1)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Row1, 0), hv_C0_Row)
                hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Col1, 0), hv_C0_Col)
                hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Row1, 1), hv_C0_Row)
                hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Col1, 1), hv_C0_Col)
                hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Row1, 2), hv_C0_Row)
                hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Col1, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = Tuple.TupleSelect(hv_Row1, hv_I1)
                hv_C1_Col = Tuple.TupleSelect(hv_Col1, hv_I1)
                hv_C2_Row = Tuple.TupleSelect(hv_Row1, hv_I2)
                hv_C2_Col = Tuple.TupleSelect(hv_Col1, hv_I2)
                hv_C3_Row = Tuple.TupleSelect(hv_Row1, hv_I3)
                hv_C3_Col = Tuple.TupleSelect(hv_Col1, hv_I3)
                hv_InRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_InCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)
                hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500.0#, 512.0#), _
                    506.0#), 506)
                hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500.0#, 500.0#), _
                    489.608), 510.392)
                Op.VectorToProjHomMat2d(hv_InRow, hv_InCol, hv_SRow, hv_SCol, "gold_standard", _
                    System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, _
                    System.DBNull.Value, System.DBNull.Value, hv_HomMat2D, hv_Covariance)
                Op.ProjectiveTransPixel(hv_HomMat2D, hv_OutRow, hv_OutCol, hv_BRow, hv_BCol)
                hv_GRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500.0#, 500.0#), _
                    500.0#), 500.0#)
                hv_GCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500.0#, 500.0#), _
                    500.0#), 500.0#)
                Op.AngleLx(hv_GRow, hv_GCol, hv_BRow, hv_BCol, hv_Angle)
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                Op.TupleDeg(CTemp(SP_C - 1), hv_Angle)
                SP_C = 0
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 180, hv_Angle)
                SP_C = 0
                Op.TupleSortIndex(hv_Angle, hv_AngIndex)
                CTemp(SP_C) = hv_Angle
                SP_C = SP_C + 1
                Op.TupleSort(CTemp(SP_C - 1), hv_Angle)
                SP_C = 0
                Op.TupleDiv(hv_Angle, 30, hv_Quot)
                Op.TupleRound(hv_Quot, hv_Int)
                Op.TupleString(hv_Int, "d", hv_STR)
                hv_CTSTRCODE = Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleSelect( _
                    hv_STR, 0), Tuple.TupleSelect(hv_STR, 1)), Tuple.TupleSelect(hv_STR, 2)), Tuple.TupleSelect( _
                    hv_STR, 3))
                Op.TupleFind(hv_CT_ALL_ID, hv_CTSTRCODE, hv_Indices)
                hv_CT_ID = Tuple.TupleAdd(hv_Indices, 1)
                'hv_O1_Row = Tuple.TupleSelect(hv_OutRow, Tuple.TupleSelect(hv_AngIndex, 0))
                'hv_O1_Col = Tuple.TupleSelect(hv_OutCol, Tuple.TupleSelect(hv_AngIndex, 0))
                'hv_O2_Row = Tuple.TupleSelect(hv_OutRow, Tuple.TupleSelect(hv_AngIndex, 1))
                'hv_O2_Col = Tuple.TupleSelect(hv_OutCol, Tuple.TupleSelect(hv_AngIndex, 1))
                'hv_O3_Row = Tuple.TupleSelect(hv_OutRow, Tuple.TupleSelect(hv_AngIndex, 2))
                'hv_O3_Col = Tuple.TupleSelect(hv_OutCol, Tuple.TupleSelect(hv_AngIndex, 2))
                'hv_O4_Row = Tuple.TupleSelect(hv_OutRow, Tuple.TupleSelect(hv_AngIndex, 3))
                'hv_O4_Col = Tuple.TupleSelect(hv_OutCol, Tuple.TupleSelect(hv_AngIndex, 3))
                hv_OutRow = Tuple.TupleSelect(hv_OutRow, hv_AngIndex)
                hv_OutCol = Tuple.TupleSelect(hv_OutCol, hv_AngIndex)

                Dim NewCT As New CodedTarget
                NewCT.CT_ID = CInt(hv_CT_ID)

                NewCT.CT_Points.Row = Tuple.TupleConcat(hv_InRow, hv_OutRow)
                NewCT.CT_Points.Col = Tuple.TupleConcat(hv_InCol, hv_OutCol)
                lstCT.Add(NewCT)
                All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
            End If
        Next
        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_ObjectSelected1)
        Marshal.ReleaseComObject(ho_RegionErosion)
        Marshal.ReleaseComObject(ho_RegionIntersection3)
        Marshal.ReleaseComObject(ho_SelectedRegions4)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_InnerTargets)
        Marshal.ReleaseComObject(ho_SelectedRegions5)
        Marshal.ReleaseComObject(ho_RegionDifference)
        Marshal.ReleaseComObject(ho_OuterTargets)
        Marshal.ReleaseComObject(ho_tempObj)


    End Sub

    Private Sub CalcGaiseki(ByVal hv_R1 As Object, ByVal hv_C1 As Object, ByVal hv_R2 As Object, ByVal hv_C2 As Object, ByRef hv_Gaiseki As Object)
        Dim Tuple As New HALCONXLib.HTupleX
        hv_Gaiseki = Tuple.TupleSub(Tuple.TupleMult(hv_R2, hv_C1), Tuple.TupleMult(hv_R1, hv_C2)) '2 つの tuple の差分を取る。
    End Sub


    ''' <summary>
    ''' ST，CTの抽出（一括処理の実行プログラム）
    ''' </summary>
    ''' Main procedure 
    Public Sub DetectTargets(ByVal ImageID As Integer, ByVal ho_Image As HALCONXLib.HUntypedObjectX, ByVal hv_CT_IDs As Object, ByVal T_Threshold As Integer)
        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Stack for temporary objects 
        Dim OTemp(10) As HUntypedObjectX
        Dim SP_O As Integer
        SP_O = 0


        ' Local iconic variables 
        Dim ho_Region As HUntypedObjectX = Nothing, ho_ConnectedRegions As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions As HUntypedObjectX = Nothing
        Dim ho_RegionFillUp As HUntypedObjectX = Nothing, ho_RegionIntersection As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions1 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions3 As HUntypedObjectX = Nothing
        Dim ho_Waku As HUntypedObjectX = Nothing, ho_FillWaku As HUntypedObjectX = Nothing
        Dim ho_Contour As HUntypedObjectX = Nothing, ho_RegressContours As HUntypedObjectX = Nothing
        Dim ho_UnionContours As HUntypedObjectX = Nothing, ho_ContoursSplit As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection1 As HUntypedObjectX = Nothing
        Dim ho_RegionDifference As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions4 As HUntypedObjectX = Nothing
        Dim ho_TransRegions As HUntypedObjectX = Nothing
        Dim ho_RegionDifference1 As HUntypedObjectX = Nothing
        Dim ho_InTarget As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected1 As HUntypedObjectX = Nothing
        Dim ho_SelectedRegionsT As HUntypedObjectX = Nothing
        Dim ho_SingleTargetRegion As HUntypedObjectX = Nothing
        Dim ho_Difference As HUntypedObjectX = Nothing
        Dim ho_CT_Regions As HUntypedObjectX = Nothing
        Dim ho_tempObj As HUntypedObjectX = Nothing
        Dim ho_ImageChannel1 As HUntypedObjectX = Nothing
        ' Local control variables 
        Dim hv_WindowHandle As Object = Nothing
        Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
        Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
        Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
        Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
        Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
        Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
        Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
        Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
        Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
        Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
        Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
        Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
        Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
        Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
        Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
        Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
        Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
        Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
        Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
        Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
        Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
        Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
        Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
        Dim hv_ST_Col As Object = Nothing

        ' Initialize local and output iconic variables 

        Op.GenEmptyObj(ho_Region)
        Op.GenEmptyObj(ho_ConnectedRegions)
        Op.GenEmptyObj(ho_SelectedRegions)
        Op.GenEmptyObj(ho_RegionFillUp)
        Op.GenEmptyObj(ho_RegionIntersection)
        Op.GenEmptyObj(ho_SelectedRegions2)
        Op.GenEmptyObj(ho_ConnectedRegions1)
        Op.GenEmptyObj(ho_ConnectedRegions2)
        Op.GenEmptyObj(ho_ObjectSelected)
        Op.GenEmptyObj(ho_ConnectedRegions3)
        Op.GenEmptyObj(ho_Waku)
        Op.GenEmptyObj(ho_FillWaku)
        Op.GenEmptyObj(ho_Contour)
        Op.GenEmptyObj(ho_RegressContours)
        Op.GenEmptyObj(ho_UnionContours)
        Op.GenEmptyObj(ho_ContoursSplit)
        Op.GenEmptyObj(ho_RegionIntersection1)
        Op.GenEmptyObj(ho_RegionDifference)
        Op.GenEmptyObj(ho_ConnectedRegions4)
        Op.GenEmptyObj(ho_RegionDifference1)
        Op.GenEmptyObj(ho_InTarget)
        Op.GenEmptyObj(ho_ObjectSelected1)

        Op.GenEmptyObj(ho_CT_Regions)
        Op.GenEmptyObj(ho_SelectedRegionsT)
        Op.GenEmptyObj(ho_SingleTargetRegion)
        Op.GenEmptyObj(ho_Difference)
        Op.GenEmptyObj(ho_tempObj)

        Op.GenEmptyObj(ho_ImageChannel1)
        All2D.Row = DBNull.Value
        All2D.Col = DBNull.Value

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        'mean_image (Image, Image, 9, 9)

        ' Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)

        'Op.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
        'Marshal.ReleaseComObject(ho_tempObj)
        'Op.BinThreshold(ho_Image, ho_tempObj)
        'Marshal.ReleaseComObject(ho_Region)
        'Op.Complement(ho_tempObj, ho_Region)
        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ImageChannel1)
        GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold)
        'Op.Threshold(ho_Image, ho_Region, 100, 255)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Op.Connection(ho_Region, ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Op.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Op.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Op.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Op.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
        Op.CountObj(ho_RegionFillUp, hv_Number)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.GenEmptyObj(ho_CT_Regions)

        For hv_Index = 1 To hv_Number Step 1
            Marshal.ReleaseComObject(ho_ObjectSelected)
            Op.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, hv_Index)
            Marshal.ReleaseComObject(ho_ConnectedRegions3)
            Op.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
            Op.CountObj(ho_ConnectedRegions3, hv_objNum)
            If Tuple.TupleEqual(hv_objNum, 5) = 1 Then
                Marshal.ReleaseComObject(ho_Waku)
                Op.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                Marshal.ReleaseComObject(ho_FillWaku)
                Op.FillUp(ho_Waku, ho_FillWaku)
                '枠の４頂点を抽出
                Marshal.ReleaseComObject(ho_Contour)
                Op.GenContourRegionXld(ho_FillWaku, ho_Contour, "border")
                'get_region_polygon (FillWaku, 5, Rows, Columns)
                'gen_contour_polygon_xld (Contour, Rows, Columns)
                '必ず四角を見つけること！！！！！！！！
                'まだまだ
                Marshal.ReleaseComObject(ho_RegressContours)
                Op.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                Marshal.ReleaseComObject(ho_UnionContours)
                Op.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                Marshal.ReleaseComObject(ho_ContoursSplit)
                Op.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                Op.LengthXld(ho_ContoursSplit, hv_Length)
                Op.TupleSortIndex(hv_Length, hv_Indices1)
                Op.TupleLastN(hv_Indices1, Tuple.TupleSub(Tuple.TupleLength(hv_Indices1), 4), hv_Selected)
                Op.TupleSort(hv_Selected, hv_Sorted1)
                Marshal.ReleaseComObject(ho_ObjectSelected1)
                Op.SelectObj(ho_ContoursSplit, ho_ObjectSelected1, Tuple.TupleAdd(hv_Sorted1, 1))

                Op.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                    hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                Op.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, Tuple.TupleConcat( _
                    Tuple.TupleSelectRange(hv_RowBegin, 1, 3), Tuple.TupleSelect(hv_RowBegin, _
                    0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_ColBegin, 1, 3), Tuple.TupleSelect( _
                    hv_ColBegin, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_RowEnd, 1, _
                    3), Tuple.TupleSelect(hv_RowEnd, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange( _
                    hv_ColEnd, 1, 3), Tuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                    hv_IsParallel)
                'tuple_first_n (Rows, |Rows|-2, Rows)
                'tuple_first_n (Columns, |Columns|-2, Columns)

                '頂点１を抽出
                'まず、頂点付近の孔を調べる。
                Marshal.ReleaseComObject(ho_RegionIntersection1)
                Op.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                Marshal.ReleaseComObject(ho_RegionDifference)
                Op.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                Marshal.ReleaseComObject(ho_ConnectedRegions4)
                Op.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                Op.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                Op.TupleSort(hv_Area, hv_Sorted)
                Op.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                Op.TupleSelect(hv_Row, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                Op.TupleSelect(hv_Column, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                '次に、その孔から一番近い頂点を頂点１とする。
                Op.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                Op.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                Op.TuplePow(hv_DiffR, 2, hv_PowR)
                Op.TuplePow(hv_DiffC, 2, hv_PowC)
                Op.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                Op.TupleSqrt(hv_Sum, hv_Distance)
                Op.TupleSortIndex(hv_Distance, hv_Indices)
                hv_tempIndex = Tuple.TupleSelect(hv_Indices, 0)
                hv_C0_Row = Tuple.TupleSelect(hv_Rows, hv_tempIndex)
                hv_C0_Col = Tuple.TupleSelect(hv_Columns, hv_tempIndex)
                'その次に、残りの３頂点を特定する。
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Columns
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = Tuple.TupleSelect(hv_Rows, hv_I1)
                hv_C1_Col = Tuple.TupleSelect(hv_Columns, hv_I1)
                hv_C2_Row = Tuple.TupleSelect(hv_Rows, hv_I2)
                hv_C2_Col = Tuple.TupleSelect(hv_Columns, hv_I2)
                hv_C3_Row = Tuple.TupleSelect(hv_Rows, hv_I3)
                hv_C3_Col = Tuple.TupleSelect(hv_Columns, hv_I3)
                hv_OutRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_OutCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)

                '枠の4頂点で射影変換
                hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500, 950), _
                    500), 950)
                hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(500, 950), _
                    950), 500)
                Op.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                    System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, _
                    System.DBNull.Value, System.DBNull.Value, hv_HomMat2D, hv_Covariance)
                'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                '枠内の4点を抽出
                Marshal.ReleaseComObject(ho_RegionDifference1)
                Op.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                Op.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                Op.TupleMean(hv_Area, hv_MeanArea)
                Marshal.ReleaseComObject(ho_InTarget)
                Op.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 8, hv_MeanArea)
                Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                'Marshal.ReleaseComObject(ho_tempObj)
                'Op.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                'Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                If Tuple.TupleLength(hv_InRow) <> 4 Then
                    Continue For
                End If
                'Op.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                '枠内の４点の配置を調べる
                Op.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                    hv_ColTrans)
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                SP_C = 0
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                SP_C = 0
                Op.TupleRound(hv_RowTrans, hv_RowIndex)
                Op.TupleRound(hv_ColTrans, hv_ColIndex)
                CTemp(SP_C) = hv_RowIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                SP_C = 0
                CTemp(SP_C) = hv_ColIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                SP_C = 0
                hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(5, Tuple.TupleSub(hv_RowIndex, _
                    1)), hv_ColIndex)
                Op.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                CTemp(SP_C) = hv_CT_Number
                SP_C = SP_C + 1
                Op.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                SP_C = 0
                hv_strCT_Number = Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd( _
                    Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleSelect(hv_CT_Number, 0), "_"), _
                    Tuple.TupleSelect(hv_CT_Number, 1)), "_"), Tuple.TupleSelect(hv_CT_Number, _
                    2)), "_"), Tuple.TupleSelect(hv_CT_Number, 3))
                '配置によって、CTの番号を特定する。
                Op.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                If hv_CT_ID_Index = -1 Then
                    ' Stop
                    Continue For
                End If
                hv_CT_ID = Tuple.TupleAdd(hv_CT_ID_Index, 1)
                If hv_CT_ID = 47 Then
                    ' Continue For
                End If
                hv_CTRow = Tuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                hv_CTCol = Tuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                Dim NewCT As New CodedTarget
                NewCT.CT_ID = CInt(hv_CT_ID)
                NewCT.ImageID1 = ImageID
                NewCT.CT_Points.Row = hv_CTRow
                NewCT.CT_Points.Col = hv_CTCol
                NewCT.SetlstCTtoST()
                lstCT.Add(NewCT)
                All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)

                Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                SP_O = SP_O + 1
                Marshal.ReleaseComObject(ho_CT_Regions)
                Op.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                Marshal.ReleaseComObject(OTemp(SP_O - 1))
                SP_O = 0

            End If

        Next
        Op.CountObj(ho_CT_Regions, hv_ST_num)
        Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)

        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0

        'Marshal.ReleaseComObject(ho_Region)
        'Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        'Marshal.ReleaseComObject(ho_ConnectedRegions)
        'Op.Connection(ho_Region, ho_ConnectedRegions)

        Marshal.ReleaseComObject(ho_Difference)
        Op.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Op.SelectShape(ho_Difference, ho_SingleTargetRegion, _
                       Tuple.TupleConcat(Tuple.TupleConcat( _
                        "holes_num", "circularity"), "area"), _
                        "and", _
                        Tuple.TupleConcat(Tuple.TupleConcat( _
                        0, 0.3), 10), _
                        Tuple.TupleConcat(Tuple.TupleConcat( _
                        0, 1), 999999))

        'VBMコードターゲットを除外する
#If 1 Then
        Dim ho_RegionDilation As HUntypedObjectX = Nothing
        Dim ho_RegionUnion As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions5 As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection2 As HUntypedObjectX = Nothing
        Op.GenEmptyObj(ho_RegionDilation)
        Marshal.ReleaseComObject(ho_RegionDilation)
        Op.GenEmptyObj(ho_RegionUnion)
        Marshal.ReleaseComObject(ho_RegionUnion)
        Op.GenEmptyObj(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Op.GenEmptyObj(ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_RegionIntersection2)

        Op.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", 3)
        Op.Union1(ho_RegionDilation, ho_RegionUnion)
        Op.Connection(ho_RegionUnion, ho_ConnectedRegions5)
        Op.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
        Op.SelectShape(ho_RegionIntersection2, ho_SingleTargetRegion, "connect_num", "and", 1, 1)
#End If
        '   Op.CountObj(ho_SingleTargetRegion, hv_ST_num)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        ' Marshal.ReleaseComObject(ho_tempObj)
        '  Op.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
        ' Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
        hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D.Row = Tuple.TupleConcat(All2D.Row, hv_ST_Rows)
        All2D.Col = Tuple.TupleConcat(All2D.Col, hv_ST_Cols)
        For hv_ind = 0 To Tuple.TupleSub(hv_ST_num, 1) Step 1
            Op.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
            Op.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
            Dim NewST As New SingleTarget
            NewST.P2D.Row = hv_ST_Row
            NewST.P2D.Col = hv_ST_Col
            NewST.P2ID = hv_ind + 1
            NewST.ImageID = ImageID
            NewST.flgUsed = 0
            lstST.Add(NewST)
        Next
        All2D_ST.Row = hv_ST_Rows
        All2D_ST.Col = hv_ST_Cols

        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Marshal.ReleaseComObject(ho_ObjectSelected)
        Marshal.ReleaseComObject(ho_ConnectedRegions3)
        Marshal.ReleaseComObject(ho_Waku)
        Marshal.ReleaseComObject(ho_FillWaku)
        Marshal.ReleaseComObject(ho_Contour)
        Marshal.ReleaseComObject(ho_RegressContours)
        Marshal.ReleaseComObject(ho_UnionContours)
        Marshal.ReleaseComObject(ho_ContoursSplit)
        Marshal.ReleaseComObject(ho_RegionIntersection1)
        Marshal.ReleaseComObject(ho_RegionDifference)
        Marshal.ReleaseComObject(ho_ConnectedRegions4)
        Marshal.ReleaseComObject(ho_RegionDifference1)
        Marshal.ReleaseComObject(ho_InTarget)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Marshal.ReleaseComObject(ho_SelectedRegionsT)
        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Marshal.ReleaseComObject(ho_Difference)
        Marshal.ReleaseComObject(ho_tempObj)
    End Sub

    ''' <summary>
    ''' ST，CTの抽出（枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更）
    ''' </summary>
    ''' Main procedure 
    ''' 枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
    ''' 
    Public Sub DetectTargetsOther(ByVal ImageID As Integer,
                                  ByVal ho_Image As HALCONXLib.HUntypedObjectX,
                                  ByVal hv_CT_IDs As Object,
                                  ByVal T_Threshold As Integer,
                                  ByVal CTargetType As Integer)



        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Stack for temporary objects 
        Dim OTemp(10) As HUntypedObjectX
        Dim SP_O As Integer
        SP_O = 0
        Dim Op As New HALCONXLib.HOperatorSetX
        Dim Tuple As New HALCONXLib.HTupleX
        ' Local iconic variables 
        Dim ho_Region As HUntypedObjectX = Nothing, ho_ConnectedRegions As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions As HUntypedObjectX = Nothing
        Dim ho_RegionFillUp As HUntypedObjectX = Nothing, ho_RegionIntersection As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions1 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions3 As HUntypedObjectX = Nothing
        Dim ho_Waku As HUntypedObjectX = Nothing, ho_FillWaku As HUntypedObjectX = Nothing
        Dim ho_Contour As HUntypedObjectX = Nothing, ho_RegressContours As HUntypedObjectX = Nothing
        Dim ho_UnionContours As HUntypedObjectX = Nothing, ho_ContoursSplit As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection1 As HUntypedObjectX = Nothing
        Dim ho_RegionDifference As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions4 As HUntypedObjectX = Nothing
        Dim ho_TransRegions As HUntypedObjectX = Nothing
        Dim ho_RegionDifference1 As HUntypedObjectX = Nothing
        Dim ho_InTarget As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected1 As HUntypedObjectX = Nothing
        Dim ho_SelectedRegionsT As HUntypedObjectX = Nothing
        Dim ho_SingleTargetRegion As HUntypedObjectX = Nothing
        Dim ho_Difference As HUntypedObjectX = Nothing
        Dim ho_CT_Regions As HUntypedObjectX = Nothing
        Dim ho_tempObj As HUntypedObjectX = Nothing
        Dim ho_ImageChannel1 As HUntypedObjectX = Nothing
        Dim ho_ContourT As HUntypedObjectX = Nothing

        ' Local control variables 
        Dim hv_WindowHandle As Object = Nothing
        Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
        Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
        Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
        Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
        Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
        Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
        Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
        Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
        Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
        Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
        Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
        Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
        Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
        Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
        Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
        Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
        Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
        Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
        Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
        Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
        Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
        Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
        Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
        Dim hv_ST_Col As Object = Nothing
        Dim hv_Length1 As Object = Nothing
        Dim hv_Indices2 As Object = Nothing
        Dim hv_Inverted As Object = Nothing
        Dim hv_Selected1 As Object = Nothing
        Dim hv_Sum1 As Object = Nothing

        Op.GenEmptyObj(ho_Region)
        Op.GenEmptyObj(ho_ConnectedRegions)
        Op.GenEmptyObj(ho_SelectedRegions)
        Op.GenEmptyObj(ho_RegionFillUp)
        Op.GenEmptyObj(ho_RegionIntersection)
        Op.GenEmptyObj(ho_SelectedRegions2)
        Op.GenEmptyObj(ho_ConnectedRegions1)
        Op.GenEmptyObj(ho_ConnectedRegions2)
        Op.GenEmptyObj(ho_ObjectSelected)
        Op.GenEmptyObj(ho_ConnectedRegions3)
        Op.GenEmptyObj(ho_Waku)
        Op.GenEmptyObj(ho_FillWaku)
        Op.GenEmptyObj(ho_Contour)
        Op.GenEmptyObj(ho_RegressContours)
        Op.GenEmptyObj(ho_UnionContours)
        Op.GenEmptyObj(ho_ContoursSplit)
        Op.GenEmptyObj(ho_RegionIntersection1)
        Op.GenEmptyObj(ho_RegionDifference)
        Op.GenEmptyObj(ho_ConnectedRegions4)
        Op.GenEmptyObj(ho_RegionDifference1)
        Op.GenEmptyObj(ho_InTarget)
        Op.GenEmptyObj(ho_ObjectSelected1)

        Op.GenEmptyObj(ho_CT_Regions)
        Op.GenEmptyObj(ho_SelectedRegionsT)
        Op.GenEmptyObj(ho_SingleTargetRegion)
        Op.GenEmptyObj(ho_Difference)
        Op.GenEmptyObj(ho_tempObj)

        Op.GenEmptyObj(ho_ImageChannel1)

        Op.GenEmptyObj(ho_ContourT)

        Op.GenEmptyObj(objTargetRegion) 'SUURI ADD 20150405

        All2D.Row = DBNull.Value
        All2D.Col = DBNull.Value

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        'mean_image (Image, Image, 9, 9)

        ' Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)

        'Op.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
        'Marshal.ReleaseComObject(ho_tempObj)
        'Op.BinThreshold(ho_Image, ho_tempObj)
        'Marshal.ReleaseComObject(ho_Region)
        'Op.Complement(ho_tempObj, ho_Region)
        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ImageChannel1)
        GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold)
        'Op.Threshold(ho_Image, ho_Region, 100, 255)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Op.Connection(ho_Region, ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Op.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Op.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Op.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Op.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
        Op.CountObj(ho_RegionFillUp, hv_Number)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.GenEmptyObj(ho_CT_Regions)

        For hv_Index = 1 To hv_Number Step 1
            Marshal.ReleaseComObject(ho_ObjectSelected)
            Op.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, hv_Index)
            Marshal.ReleaseComObject(ho_ConnectedRegions3)
            Op.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
            Op.CountObj(ho_ConnectedRegions3, hv_objNum)
            If Tuple.TupleEqual(hv_objNum, 5) = 1 Then
                Marshal.ReleaseComObject(ho_Waku)
                Op.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                Marshal.ReleaseComObject(ho_FillWaku)
                Op.FillUp(ho_Waku, ho_FillWaku)
                '枠の４頂点を抽出
                '枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
                Marshal.ReleaseComObject(ho_ContourT)
                Op.GenContourRegionXld(ho_Waku, ho_ContourT, "border_holes")
                Op.LengthXld(ho_ContourT, hv_Length1)
                Op.TupleSortIndex(hv_Length1, hv_Indices2)
                Op.TupleInverse(hv_Indices2, hv_Inverted)
                Op.TupleSelect(hv_Inverted, 1, hv_Selected1)
                Op.TupleAdd(hv_Selected1, 1, hv_Sum1)
                Marshal.ReleaseComObject(ho_Contour)
                Op.SelectObj(ho_ContourT, ho_Contour, hv_Sum1)

                'get_region_polygon (FillWaku, 5, Rows, Columns)
                'gen_contour_polygon_xld (Contour, Rows, Columns)
                '必ず四角を見つけること！！！！！！！！
                'まだまだ
                Marshal.ReleaseComObject(ho_RegressContours)
                Op.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                Marshal.ReleaseComObject(ho_UnionContours)
                Op.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                Marshal.ReleaseComObject(ho_ContoursSplit)
                Op.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                Op.LengthXld(ho_ContoursSplit, hv_Length)
                Op.TupleSortIndex(hv_Length, hv_Indices1)
                Op.TupleLastN(hv_Indices1, Tuple.TupleSub(Tuple.TupleLength(hv_Indices1), 4), hv_Selected)
                Op.TupleSort(hv_Selected, hv_Sorted1)
                Marshal.ReleaseComObject(ho_ObjectSelected1)

                Dim hv_Selected_L As Object = Nothing
                hv_Selected_L = Tuple.TupleAdd(hv_Sorted1, 1)

                Try

                    Dim i As Integer
                    Op.GenEmptyObj(ho_ObjectSelected1)
                    For i = 1 To Tuple.TupleLength(hv_Selected_L)
                        Dim ho_tmpobj As HUntypedObjectX = Nothing
                        Op.GenEmptyObj(ho_tmpobj)
                        Op.SelectObj(ho_ContoursSplit, ho_tmpobj, hv_Selected_L(i - 1))
                        Op.ConcatObj(ho_ObjectSelected1, ho_tmpobj, ho_ObjectSelected1)
                        Marshal.ReleaseComObject(ho_tmpobj)
                    Next


                Catch ex As Exception
                    Dim sss As String = ""
                    sss = ex.Message
                End Try


                Op.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                    hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                Op.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, Tuple.TupleConcat( _
                    Tuple.TupleSelectRange(hv_RowBegin, 1, 3), Tuple.TupleSelect(hv_RowBegin, _
                    0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_ColBegin, 1, 3), Tuple.TupleSelect( _
                    hv_ColBegin, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_RowEnd, 1, _
                    3), Tuple.TupleSelect(hv_RowEnd, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange( _
                    hv_ColEnd, 1, 3), Tuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                    hv_IsParallel)
                'tuple_first_n (Rows, |Rows|-2, Rows)
                'tuple_first_n (Columns, |Columns|-2, Columns)

                '頂点１を抽出
                'まず、頂点付近の孔を調べる。
                Marshal.ReleaseComObject(ho_RegionIntersection1)
                Op.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                Marshal.ReleaseComObject(ho_RegionDifference)
                Op.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                Marshal.ReleaseComObject(ho_ConnectedRegions4)
                Op.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                Op.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                Op.TupleSort(hv_Area, hv_Sorted)
                Op.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                Op.TupleSelect(hv_Row, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                Op.TupleSelect(hv_Column, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                '次に、その孔から一番近い頂点を頂点１とする。
                Op.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                Op.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                Op.TuplePow(hv_DiffR, 2, hv_PowR)
                Op.TuplePow(hv_DiffC, 2, hv_PowC)
                Op.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                Op.TupleSqrt(hv_Sum, hv_Distance)
                Op.TupleSortIndex(hv_Distance, hv_Indices)
                hv_tempIndex = Tuple.TupleSelect(hv_Indices, 0)
                hv_C0_Row = Tuple.TupleSelect(hv_Rows, hv_tempIndex)
                hv_C0_Col = Tuple.TupleSelect(hv_Columns, hv_tempIndex)
                'その次に、残りの３頂点を特定する。
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Columns
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = Tuple.TupleSelect(hv_Rows, hv_I1)
                hv_C1_Col = Tuple.TupleSelect(hv_Columns, hv_I1)
                hv_C2_Row = Tuple.TupleSelect(hv_Rows, hv_I2)
                hv_C2_Col = Tuple.TupleSelect(hv_Columns, hv_I2)
                hv_C3_Row = Tuple.TupleSelect(hv_Rows, hv_I3)
                hv_C3_Col = Tuple.TupleSelect(hv_Columns, hv_I3)
                hv_OutRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_OutCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)

                '枠の4頂点で射影変換
                hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550, 900), _
                    550), 900)
                If CTargetType = 0 Then
                    hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550, 900), _
                        900), 550)
                Else
                    hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550 + 7.855, 900), _
                      900 + 7.855), 550)
                End If

                Op.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                    System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, _
                    System.DBNull.Value, System.DBNull.Value, hv_HomMat2D, hv_Covariance)
                'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                '枠内の4点を抽出
                Marshal.ReleaseComObject(ho_RegionDifference1)
                Op.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                Op.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                Op.TupleMean(hv_Area, hv_MeanArea)
                Marshal.ReleaseComObject(ho_InTarget)
                Op.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 15, hv_MeanArea)
                Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                'Marshal.ReleaseComObject(ho_tempObj)
                'Op.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                'Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                If Tuple.TupleLength(hv_InRow) <> 4 Then
                    Continue For
                End If
                'Op.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                '枠内の４点の配置を調べる
                Op.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                    hv_ColTrans)
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                SP_C = 0
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                SP_C = 0
                Op.TupleRound(hv_RowTrans, hv_RowIndex)
                Op.TupleRound(hv_ColTrans, hv_ColIndex)
                CTemp(SP_C) = hv_RowIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                SP_C = 0
                CTemp(SP_C) = hv_ColIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                SP_C = 0
                hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(5, Tuple.TupleSub(hv_RowIndex, _
                    1)), hv_ColIndex)
                Op.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                CTemp(SP_C) = hv_CT_Number
                SP_C = SP_C + 1
                Op.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                SP_C = 0
                hv_strCT_Number = Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd( _
                    Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleSelect(hv_CT_Number, 0), "_"), _
                    Tuple.TupleSelect(hv_CT_Number, 1)), "_"), Tuple.TupleSelect(hv_CT_Number, _
                    2)), "_"), Tuple.TupleSelect(hv_CT_Number, 3))
                '配置によって、CTの番号を特定する。
                Op.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                If hv_CT_ID_Index = -1 Then
                    ' Stop
                    Continue For
                End If
                hv_CT_ID = Tuple.TupleAdd(hv_CT_ID_Index, 1)
                If hv_CT_ID = 47 Then
                    ' Continue For
                End If
                hv_CTRow = Tuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                hv_CTCol = Tuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                '  Op.ConcatObj(objTargetRegion, ho_InTarget, objTargetRegion) 'SUURI ADD 20150404
                If hv_CT_ID < 419 Then


                    Dim NewCT As New CodedTarget
                    NewCT.CT_ID = CInt(hv_CT_ID)
                    NewCT.ImageID1 = ImageID
                    NewCT.CT_Points.Row = hv_CTRow
                    NewCT.CT_Points.Col = hv_CTCol
                    NewCT.AllST_Area = Tuple.TupleSum(hv_AreaST) 'SUURI ADD 20150217
                    NewCT.SetlstCTtoST()
                    Dim BadCT As Boolean = False
                    For Each CT As CodedTarget In lstCT
                        If CT.CT_ID = NewCT.CT_ID Then
                            BadCT = True
                            Exit For

                        End If
                    Next
                    If BadCT = False Then
                        lstCT.Add(NewCT)
                        All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                        All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)

                    End If
                    Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                    SP_O = SP_O + 1
                    Marshal.ReleaseComObject(ho_CT_Regions)
                    Op.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                    Marshal.ReleaseComObject(OTemp(SP_O - 1))
                    SP_O = 0
                End If
            End If

        Next
        Op.CountObj(ho_CT_Regions, hv_ST_num)
        Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)

        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0

        'Marshal.ReleaseComObject(ho_Region)
        'Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        'Marshal.ReleaseComObject(ho_ConnectedRegions)
        'Op.Connection(ho_Region, ho_ConnectedRegions)

        Marshal.ReleaseComObject(ho_Difference)
        Op.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
        Op.FillUp(ho_Difference, ho_Difference) 'SUSANO ADD 20150917 
        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Dim param1 As Object = Nothing 'タプル結合だけしており、処理の無駄
        Dim parammin As Object = Nothing
        Dim parammax As Object = Nothing
        param1 = Tuple.TupleConcat(Tuple.TupleConcat("holes_num", "circularity"), "area")
        parammin = Tuple.TupleConcat(Tuple.TupleConcat(0, 0.3), 10.0)
        parammax = Tuple.TupleConcat(Tuple.TupleConcat(0, 1.0), 999999.0)

        Op.SelectShape(ho_Difference, ho_SingleTargetRegion, "holes_num", "and", 0, 0)
        Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "circularity", "and", 0.3, 1.0)
        Dim minAreaFourTen As Integer = CInt(ReadFourTenTarget(6))
        Dim maxAreaFourTen As Integer = CInt(ReadFourTenTarget(7))
        '20170719 kiryu Debug 999.0→99999.0 室内実験用
        Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", minAreaFourTen, maxAreaFourTen) ' SUSANO UPDATE 20151018 99999.0->999.0に変更

        '   Op.SelectShape(ho_Difference, ho_SingleTargetRegion, param1, "and", parammin, parammax)

        'Op.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '              Tuple.TupleConcat(Tuple.TupleConcat( _
        '               "holes_num", "circularity"), "area"), _
        '               "and", _
        '               Tuple.TupleConcat(Tuple.TupleConcat( _
        '               0, 0.3), 10), _
        '               Tuple.TupleConcat(Tuple.TupleConcat( _
        '               0, 1.0), 999999))
        'Op.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '             Tuple.TupleConcat("holes_num", "area"), _
        '              "and", Tuple.TupleConcat(0, 10), Tuple.TupleConcat(0, 999999))

        'VBMコードターゲットを除外する
#If True Then
        Dim ho_RegionDilation As HUntypedObjectX = Nothing
        Dim ho_RegionUnion As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions5 As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection2 As HUntypedObjectX = Nothing
        Dim ho_NonVBMCTRegion As HUntypedObjectX = Nothing
        Dim ho_objFourTenTarget As HUntypedObjectX = Nothing, ho_objConnectionFourTenTarget As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions1 As HUntypedObjectX = Nothing, ho_SelectedRegions3 As HUntypedObjectX = Nothing

        Dim hv_FourTen_num As Object = Nothing, hv_Index1 As Object = Nothing
        Dim hv_Number1 As Object = Nothing


        Op.GenEmptyObj(ho_RegionDilation)
        Marshal.ReleaseComObject(ho_RegionDilation)
        Op.GenEmptyObj(ho_RegionUnion)
        Marshal.ReleaseComObject(ho_RegionUnion)
        Op.GenEmptyObj(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Op.GenEmptyObj(ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_RegionIntersection2)
        Op.GenEmptyObj(ho_NonVBMCTRegion)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)
        'SUSANO ADD START 20151018
        ' Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", 1.0, 500)
        'SUSANO ADD END 20151018
        Dim dilationRadius As Integer = 3
        dilationRadius = CInt(ReadFourTenTarget(5))
        Op.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", dilationRadius)   '20170716 Kiryu 3→9　室内実験用
        Op.Union1(ho_RegionDilation, ho_RegionUnion)
        Op.Connection(ho_RegionUnion, ho_ConnectedRegions5)
        Op.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
        Op.SelectShape(ho_RegionIntersection2, ho_NonVBMCTRegion, "connect_num", "and", 4, 4)

        Op.CountObj(ho_NonVBMCTRegion, hv_FourTen_num)


        Dim win As New TrgtDtctDebugWin

        If ImageID = 8 Then
            win.Show()
            win.SetTitle(ImageID, T_Threshold)
            win.Disp(ho_Image, Nothing)
        End If

        win.DispReg(ho_NonVBMCTRegion)


        For hv_Index1 = 1 To hv_FourTen_num Step 1

            Call Op.SelectObj(ho_NonVBMCTRegion, ho_objFourTenTarget, hv_Index1)
            Call Op.Connection(ho_objFourTenTarget, ho_objConnectionFourTenTarget)

            Call Op.SelectShape(ho_objConnectionFourTenTarget, ho_SelectedRegions1, "area", _
                "and", minAreaFourTen, maxAreaFourTen)

            '真円度0.7以上
            Call Op.SelectShape(ho_SelectedRegions1, ho_SelectedRegions3, "circularity", _
                "and", 0.7, 1)

            Call Op.CountObj(ho_SelectedRegions3, hv_Number1)

            If Tuple.TupleEqual(hv_Number1, 4) Then

                Get2DCoord(ho_ImageChannel1, ho_SelectedRegions3, hv_CTRow, hv_CTCol, hv_AreaST)

                Dim hvAreaSort As Object = Nothing
                hvAreaSort = Tuple.TupleSortIndex(hv_AreaST)
                '20150914 UPDATE START SUSANO 4tenTargetの大３点の面積の比率を考慮する。
                Dim hvArea012 As Object = Nothing
                Dim hvArea012s As Object = Nothing
                hvArea012s = Tuple.TupleConcat(Tuple.TupleConcat(hv_AreaST(hvAreaSort(1)), hv_AreaST(hvAreaSort(2))), hv_AreaST(hvAreaSort(3)))
                hvArea012 = Tuple.TupleMean(hvArea012s)
                Dim hvArea012_dev As Object = Nothing
                hvArea012_dev = Tuple.TupleDeviation(hvArea012s)

                Dim hvArea3 As Object = Nothing
                hvArea3 = hv_AreaST(hvAreaSort(0))
                Debug.Print("camID=" & ImageID & " 平均面積=" & hvArea012 & "," & hvArea012_dev & vbNewLine)
                My.Computer.FileSystem.WriteAllText("4tenTargetMonitor.txt", "camID=" & ImageID & " 平均面積=" & hvArea012 & "," & hvArea012_dev & vbNewLine, True)
                Dim dblsetAreaHiritu As Double = CDbl(ReadFourTenTarget(1))
                Dim dblsetArea012 As Double = CDbl(ReadFourTenTarget(2))
                Dim dblsetArea012_dev As Double = CDbl(ReadFourTenTarget(3))
                If hvArea012 * dblsetAreaHiritu > hvArea3 And hvArea012 > dblsetArea012 And hvArea012_dev < dblsetArea012_dev Then
                    '20150915 UPDATE END SUSANO 4tenTargetの大３点の面積の比率を考慮する。

                    Dim R0 As Object = Nothing
                    Dim C0 As Object = Nothing
                    Dim R1 As Object = Nothing
                    Dim C1 As Object = Nothing
                    Dim R2 As Object = Nothing
                    Dim C2 As Object = Nothing
                    Dim R3 As Object = Nothing
                    Dim C3 As Object = Nothing
                    FourTenTarget1234(hv_CTRow, hv_CTCol, hvAreaSort(0), R0, C0, R1, C1, R2, C2, R3, C3)
                    hv_CTRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(R0, R1), R2), R3)
                    hv_CTCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(C0, C1), C2), C3)

                    hv_OutRow = Tuple.TupleConcat(Tuple.TupleConcat(R0, R1), R2)
                    hv_OutCol = Tuple.TupleConcat(Tuple.TupleConcat(C0, C1), C2)

                    '大3点で射影変換
                    hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(0, 0), 400)
                    hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(0, 400), 0)
                    Op.VectorToHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, hv_HomMat2D)

                    Op.AffineTransPoint2D(hv_HomMat2D, R3, C3, hv_RowTrans, hv_ColTrans)
                    'R3 = hv_RowTrans(3) - 280
                    'C3 = hv_ColTrans(3) - 280

                    CTemp(SP_C) = hv_RowTrans
                    SP_C = SP_C + 1
                    Op.TupleSub(CTemp(SP_C - 1), 280, hv_RowTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColTrans
                    SP_C = SP_C + 1
                    Op.TupleSub(CTemp(SP_C - 1), 280, hv_ColTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_RowTrans
                    SP_C = SP_C + 1
                    Op.TupleDiv(CTemp(SP_C - 1), 120, hv_RowTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColTrans
                    SP_C = SP_C + 1
                    Op.TupleDiv(CTemp(SP_C - 1), 120, hv_ColTrans)
                    SP_C = 0
                    Op.TupleRound(hv_RowTrans, hv_RowIndex)
                    Op.TupleRound(hv_ColTrans, hv_ColIndex)
                    CTemp(SP_C) = hv_RowIndex
                    SP_C = SP_C + 1
                    Op.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColIndex
                    SP_C = SP_C + 1
                    Op.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                    SP_C = 0
                    If hv_RowIndex = 0 Or hv_ColIndex = 0 Then
                        If hv_RowIndex = 0 Then
                            hv_CT_Number = 17
                        End If
                        If hv_ColIndex = 0 Then
                            hv_CT_Number = 18
                        End If
                    Else
                        hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(4, Tuple.TupleSub(hv_RowIndex, _
                      1)), hv_ColIndex)
                    End If

                    Op.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                    CTemp(SP_C) = hv_CT_Number
                    SP_C = SP_C + 1
                    Op.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                    SP_C = 0
                    hv_strCT_Number = hv_CT_Number
                    If ImageID = 11 And hv_CT_Number = 11 Then
                        Debug.Print("da")
                    End If
                    If hv_CT_Number < 19 And hv_CT_Number > 0 Then
                        hv_strCT_Number = hv_CT_Number

                        Dim NewCT As New CodedTarget
                        NewCT.CT_ID = CInt(hv_CT_Number) + 400
                        NewCT.ImageID1 = ImageID
                        NewCT.CT_Points.Row = hv_CTRow
                        NewCT.CT_Points.Col = hv_CTCol
                        NewCT.AllST_Area = Tuple.TupleSum(hv_AreaST) 'SUURI ADD 20150217
                        NewCT.SetlstCTtoST()
                        lstCT.Add(NewCT)
                        All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                        All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                    End If
                End If
            End If

        Next

        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)
#End If
        '   Op.CountObj(ho_SingleTargetRegion, hv_ST_num)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        ' Marshal.ReleaseComObject(ho_tempObj)
        '  Op.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
        ' Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
        Op.WriteRegion(ho_SingleTargetRegion, "SingleTargetRegion" & ImageID & ".reg")
        '  Op.ConcatObj(objTargetRegion, ho_SingleTargetRegion, objTargetRegion) 'SUURI ADD 20150404

        hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D.Row = Tuple.TupleConcat(All2D.Row, hv_ST_Rows)
        All2D.Col = Tuple.TupleConcat(All2D.Col, hv_ST_Cols)

        '基準点ターゲットを大白○だけで認識
        Dim intHanteiKyori As Integer = 10

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt")
        If IsNumeric(fileContents.Split(" ")(1)) Then
            intHanteiKyori = CInt(fileContents.Split(" ")(1))
        End If
        Dim dblsetAreaBigST As Double = CDbl(ReadFourTenTarget(4))
        For Each objtmpTT As CodedTarget In objBeforeTarget.lstCT
            If ImageID = 7 And objtmpTT.CT_ID = 195 Then
                Debug.Print("stop")
            End If
            Dim objMinKK As Double = Double.MaxValue
            Dim objMinKK_ST As SingleTarget = Nothing
            hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
            For hv_ind = 0 To Tuple.TupleSub(hv_ST_num, 1) Step 1
                Op.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
                Op.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
                Dim NewST As New SingleTarget
                NewST.P2D.Row = hv_ST_Row
                NewST.P2D.Col = hv_ST_Col
                NewST.P2ID = hv_ind + 1
                NewST.ImageID = ImageID
                NewST.flgUsed = 0
                Op.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
                '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217

                Dim tmpKK As Object = Nothing
                objtmpTT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(hv_ST_Row, hv_ST_Col, tmpKK)
                If objMinKK > tmpKK Then
                    objMinKK = tmpKK
                    objMinKK_ST = NewST
                End If
            Next

            If objMinKK < intHanteiKyori And objMinKK_ST.AreaST >= dblsetAreaBigST Then
                'Op.TupleRemove(hv_ST_Rows, objMinKK_ST.P2ID - 1, hv_ST_Rows)
                'Op.TupleRemove(hv_ST_Cols, objMinKK_ST.P2ID - 1, hv_ST_Cols)
                Dim NewCT As New CodedTarget
                NewCT.CT_ID = objtmpTT.CT_ID
                NewCT.ImageID1 = objtmpTT.ImageID1
                'Dim oneSTtoCT As New ImagePoints
                'oneSTtoCT.Row = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                'oneSTtoCT.Col = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))

                NewCT.CT_Points.Row = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                NewCT.CT_Points.Col = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))
                NewCT.AllST_Area = objMinKK_ST.AreaST
                NewCT.SetlstCTtoST()
                lstCT.Add(NewCT)
            End If
        Next

        hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
        For hv_ind = 0 To Tuple.TupleSub(hv_ST_num, 1) Step 1
            Op.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
            Op.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
            Dim NewST As New SingleTarget
            NewST.P2D.Row = hv_ST_Row
            NewST.P2D.Col = hv_ST_Col
            NewST.P2ID = hv_ind + 1
            NewST.ImageID = ImageID
            NewST.flgUsed = 0
            Op.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
            '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217
            lstST.Add(NewST)
        Next

        'SUURI ADD 20150217
#If True Then
        insertSTtoCTpoint()

        'For Each objCT As CodedTarget In lstCT
        '    Dim mindistST As Double = Double.MaxValue
        '    Dim minST As SingleTarget = Nothing
        '    '対象のＣＴに一番近いＳＴを取得
        '    For Each objST As SingleTarget In lstST
        '        Dim objKyori As Object = Nothing
        '        'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
        '        'つまり、ある程度大きさのあるＳＴを選択する。
        '        If objST.AreaST > objCT.AllST_Area Then
        '            objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
        '                                                             objST.P2D.Col,
        '                                                             objKyori)
        '            If objKyori < mindistST Then
        '                mindistST = objKyori
        '                minST = objST
        '            End If
        '        End If
        '    Next
        '    '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
        '    'つまりある程度CTに近いＳＴを選択する。
        '    If mindistST < objCT.GetAllPointsKyoriPixel Then

        '        objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
        '        objCT.CT_Points.Row(0) = minST.P2D.Row
        '        objCT.CT_Points.Col(0) = minST.P2D.Col
        '        objCT.CenterPoint.CopyToMe(minST.P2D)

        '    End If
        'Next

#End If
        'SUURI ADD 20150217 
        All2D_ST.Row = hv_ST_Rows
        All2D_ST.Col = hv_ST_Cols

        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Marshal.ReleaseComObject(ho_ObjectSelected)
        Marshal.ReleaseComObject(ho_ConnectedRegions3)
        Marshal.ReleaseComObject(ho_Waku)
        Marshal.ReleaseComObject(ho_FillWaku)
        Marshal.ReleaseComObject(ho_Contour)
        Marshal.ReleaseComObject(ho_RegressContours)
        Marshal.ReleaseComObject(ho_UnionContours)
        Marshal.ReleaseComObject(ho_ContoursSplit)
        Marshal.ReleaseComObject(ho_RegionIntersection1)
        Marshal.ReleaseComObject(ho_RegionDifference)
        Marshal.ReleaseComObject(ho_ConnectedRegions4)
        Marshal.ReleaseComObject(ho_RegionDifference1)
        Marshal.ReleaseComObject(ho_InTarget)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Marshal.ReleaseComObject(ho_SelectedRegionsT)

        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Marshal.ReleaseComObject(ho_Difference)
        Marshal.ReleaseComObject(ho_tempObj)
        Marshal.ReleaseComObject(ho_ContourT)
    End Sub


    Public Sub DetectTargetsOther2(ByVal ImageID As Integer,
                              ByVal ho_Image As HALCONXLib.HUntypedObjectX,
                              ByVal hv_CT_IDs As Object,
                              ByVal T_Threshold As Integer,
                              ByVal CTargetType As Integer,
                              ByVal TGDetectPram As TargetDetectParam)

        '#Const DebugWindow = 1

        '#If DebugWindow = 1 Then

        '        Dim hv_WindowID As HTuple = New HTuple

        '        If (HDevWindowStack.IsOpen()) Then
        '            HOperatorSet.CloseWindow(HDevWindowStack.Pop())
        '        End If
        '        HOperatorSet.SetWindowAttr("background_color", New HTuple("black"))
        '        HOperatorSet.OpenWindow(New HTuple(0), New HTuple(0), New HTuple(960), New HTuple(687), 0, "", "", hv_WindowID)
        '        HOperatorSet.SetPart(hv_WindowID, 0, 0, 2748, 3840)
        '        HDevWindowStack.Push(hv_WindowID)

        '        set_display_font(hv_WindowID, New HTuple(12), New HTuple("mono"), New HTuple("true"), _
        '            New HTuple("false"))



        '        If (HDevWindowStack.IsOpen()) Then
        '            HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive())
        '        End If
        '        If (HDevWindowStack.IsOpen()) Then
        '            HOperatorSet.SetColor(HDevWindowStack.GetActive(), New HTuple("green"))
        '        End If
        '        If (HDevWindowStack.IsOpen()) Then
        '            HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), New HTuple(3))
        '        End If
        '        If (HDevWindowStack.IsOpen()) Then
        '            HOperatorSet.SetDraw(HDevWindowStack.GetActive(), New HTuple("margin"))
        '        End If


        '#End If


        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Stack for temporary objects 
        Dim OTemp(10) As HUntypedObjectX
        Dim SP_O As Integer
        SP_O = 0
        Dim Op As New HALCONXLib.HOperatorSetX
        Dim Tuple As New HALCONXLib.HTupleX
        ' Local iconic variables 
        Dim ho_Region As HUntypedObjectX = Nothing, ho_ConnectedRegions As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions As HUntypedObjectX = Nothing
        Dim ho_RegionFillUp As HUntypedObjectX = Nothing, ho_RegionIntersection As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions1 As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions2 As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions3 As HUntypedObjectX = Nothing
        Dim ho_Waku As HUntypedObjectX = Nothing, ho_FillWaku As HUntypedObjectX = Nothing
        Dim ho_Contour As HUntypedObjectX = Nothing, ho_RegressContours As HUntypedObjectX = Nothing
        Dim ho_UnionContours As HUntypedObjectX = Nothing, ho_ContoursSplit As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection1 As HUntypedObjectX = Nothing
        Dim ho_RegionDifference As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions4 As HUntypedObjectX = Nothing
        Dim ho_TransRegions As HUntypedObjectX = Nothing
        Dim ho_RegionDifference1 As HUntypedObjectX = Nothing
        Dim ho_InTarget As HUntypedObjectX = Nothing
        Dim ho_ObjectSelected1 As HUntypedObjectX = Nothing
        Dim ho_SelectedRegionsT As HUntypedObjectX = Nothing
        Dim ho_SingleTargetRegion As HUntypedObjectX = Nothing
        Dim ho_Difference As HUntypedObjectX = Nothing
        Dim ho_CT_Regions As HUntypedObjectX = Nothing
        Dim ho_tempObj As HUntypedObjectX = Nothing
        Dim ho_ImageChannel1 As HUntypedObjectX = Nothing
        Dim ho_ContourT As HUntypedObjectX = Nothing

        ' Local control variables 
        Dim hv_WindowHandle As Object = Nothing
        Dim hv_Number As Object = Nothing, hv_Index As Object = Nothing
        Dim hv_objNum As Object = Nothing, hv_Rows As Object = Nothing
        Dim hv_Columns As Object = Nothing, hv_RowBegin As Object = Nothing
        Dim hv_ColBegin As Object = Nothing, hv_RowEnd As Object = Nothing
        Dim hv_ColEnd As Object = Nothing, hv_Nr As Object = Nothing
        Dim hv_Nc As Object = Nothing, hv_Dist As Object = Nothing
        Dim hv_IsParallel As Object = Nothing, hv_Area As Object = Nothing
        Dim hv_Row As Object = Nothing, hv_Column As Object = Nothing
        Dim hv_Sorted As Object = Nothing, hv_SortedAreaIndex As Object = Nothing
        Dim hv_AnaRow As Object = Nothing, hv_AnaColumn As Object = Nothing
        Dim hv_DiffR As Object = Nothing, hv_DiffC As Object = Nothing
        Dim hv_PowR As Object = Nothing, hv_PowC As Object = Nothing
        Dim hv_Sum As Object = Nothing, hv_Distance As Object = Nothing
        Dim hv_Indices As Object = Nothing, hv_tempIndex As Object = Nothing
        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing, hv_OutRow As Object = Nothing
        Dim hv_OutCol As Object = Nothing, hv_SRow As Object = Nothing
        Dim hv_SCol As Object = Nothing, hv_HomMat2D As Object = Nothing
        Dim hv_Covariance As Object = Nothing, hv_MeanArea As Object = Nothing
        Dim hv_InArea As Object = Nothing, hv_InRow As Object = Nothing
        Dim hv_InColumn As Object = Nothing, hv_RowTrans As Object = Nothing
        Dim hv_ColTrans As Object = Nothing, hv_RowIndex As Object = Nothing
        Dim hv_ColIndex As Object = Nothing, hv_CT_Number As Object = Nothing
        Dim hv_strCT_Number As Object = Nothing, hv_CT_ID_Index As Object = Nothing
        Dim hv_CT_ID As Object = Nothing, hv_Length As Object = Nothing
        Dim hv_Indices1 As Object = Nothing, hv_Selected As Object = Nothing
        Dim hv_Sorted1 As Object = Nothing, hv_CT_NumberIndex As Object = Nothing
        Dim hv_CTRow As Object = Nothing, hv_CTCol As Object = Nothing
        Dim hv_ST_num As Object = Nothing, hv_AreaST As Object = Nothing
        Dim hv_ind As Object = Nothing, hv_ST_Rows As Object = Nothing
        Dim hv_ST_Cols As Object = Nothing, hv_ST_Row As Object = Nothing
        Dim hv_ST_Col As Object = Nothing
        Dim hv_Length1 As Object = Nothing
        Dim hv_Indices2 As Object = Nothing
        Dim hv_Inverted As Object = Nothing
        Dim hv_Selected1 As Object = Nothing
        Dim hv_Sum1 As Object = Nothing

        Op.GenEmptyObj(ho_Region)
        Op.GenEmptyObj(ho_ConnectedRegions)
        Op.GenEmptyObj(ho_SelectedRegions)
        Op.GenEmptyObj(ho_RegionFillUp)
        Op.GenEmptyObj(ho_RegionIntersection)
        Op.GenEmptyObj(ho_SelectedRegions2)
        Op.GenEmptyObj(ho_ConnectedRegions1)
        Op.GenEmptyObj(ho_ConnectedRegions2)
        Op.GenEmptyObj(ho_ObjectSelected)
        Op.GenEmptyObj(ho_ConnectedRegions3)
        Op.GenEmptyObj(ho_Waku)
        Op.GenEmptyObj(ho_FillWaku)
        Op.GenEmptyObj(ho_Contour)
        Op.GenEmptyObj(ho_RegressContours)
        Op.GenEmptyObj(ho_UnionContours)
        Op.GenEmptyObj(ho_ContoursSplit)
        Op.GenEmptyObj(ho_RegionIntersection1)
        Op.GenEmptyObj(ho_RegionDifference)
        Op.GenEmptyObj(ho_ConnectedRegions4)
        Op.GenEmptyObj(ho_RegionDifference1)
        Op.GenEmptyObj(ho_InTarget)
        Op.GenEmptyObj(ho_ObjectSelected1)

        Op.GenEmptyObj(ho_CT_Regions)
        Op.GenEmptyObj(ho_SelectedRegionsT)
        Op.GenEmptyObj(ho_SingleTargetRegion)
        Op.GenEmptyObj(ho_Difference)
        Op.GenEmptyObj(ho_tempObj)

        Op.GenEmptyObj(ho_ImageChannel1)

        Op.GenEmptyObj(ho_ContourT)

        Op.GenEmptyObj(objTargetRegion) 'SUURI ADD 20150405

        All2D.Row = DBNull.Value
        All2D.Col = DBNull.Value

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        'mean_image (Image, Image, 9, 9)

        ' Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)

        'Op.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
        'Marshal.ReleaseComObject(ho_tempObj)
        'Op.BinThreshold(ho_Image, ho_tempObj)
        'Marshal.ReleaseComObject(ho_Region)
        'Op.Complement(ho_tempObj, ho_Region)
        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ImageChannel1)
        GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold)
        'Op.Threshold(ho_Image, ho_Region, 100, 255)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Op.Connection(ho_Region, ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Op.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Op.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Op.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Op.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Op.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Op.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
        Op.CountObj(ho_RegionFillUp, hv_Number)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.GenEmptyObj(ho_CT_Regions)


        For hv_Index = 1 To hv_Number Step 1
            Marshal.ReleaseComObject(ho_ObjectSelected)
            Op.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, hv_Index)
            Marshal.ReleaseComObject(ho_ConnectedRegions3)
            Op.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
            Op.CountObj(ho_ConnectedRegions3, hv_objNum)
            If Tuple.TupleEqual(hv_objNum, 5) = 1 Then
                Marshal.ReleaseComObject(ho_Waku)
                Op.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                Marshal.ReleaseComObject(ho_FillWaku)
                Op.FillUp(ho_Waku, ho_FillWaku)
                '枠の４頂点を抽出
                '枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
                Marshal.ReleaseComObject(ho_ContourT)
                Op.GenContourRegionXld(ho_Waku, ho_ContourT, "border_holes")
                Op.LengthXld(ho_ContourT, hv_Length1)
                Op.TupleSortIndex(hv_Length1, hv_Indices2)
                Op.TupleInverse(hv_Indices2, hv_Inverted)
                Op.TupleSelect(hv_Inverted, 1, hv_Selected1)
                Op.TupleAdd(hv_Selected1, 1, hv_Sum1)
                Marshal.ReleaseComObject(ho_Contour)
                Op.SelectObj(ho_ContourT, ho_Contour, hv_Sum1)

                'get_region_polygon (FillWaku, 5, Rows, Columns)
                'gen_contour_polygon_xld (Contour, Rows, Columns)
                '必ず四角を見つけること！！！！！！！！
                'まだまだ
                Marshal.ReleaseComObject(ho_RegressContours)
                Op.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                Marshal.ReleaseComObject(ho_UnionContours)
                Op.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                Marshal.ReleaseComObject(ho_ContoursSplit)
                Op.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                Op.LengthXld(ho_ContoursSplit, hv_Length)
                Op.TupleSortIndex(hv_Length, hv_Indices1)
                Op.TupleLastN(hv_Indices1, Tuple.TupleSub(Tuple.TupleLength(hv_Indices1), 4), hv_Selected)
                Op.TupleSort(hv_Selected, hv_Sorted1)
                Marshal.ReleaseComObject(ho_ObjectSelected1)

                Dim hv_Selected_L As Object = Nothing
                hv_Selected_L = Tuple.TupleAdd(hv_Sorted1, 1)

                Try

                    Dim i As Integer
                    Op.GenEmptyObj(ho_ObjectSelected1)
                    For i = 1 To Tuple.TupleLength(hv_Selected_L)
                        Dim ho_tmpobj As HUntypedObjectX = Nothing
                        Op.GenEmptyObj(ho_tmpobj)
                        Op.SelectObj(ho_ContoursSplit, ho_tmpobj, hv_Selected_L(i - 1))
                        Op.ConcatObj(ho_ObjectSelected1, ho_tmpobj, ho_ObjectSelected1)
                        Marshal.ReleaseComObject(ho_tmpobj)
                    Next


                Catch ex As Exception
                    Dim sss As String = ""
                    sss = ex.Message
                End Try


                Op.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                    hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                Op.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, Tuple.TupleConcat( _
                    Tuple.TupleSelectRange(hv_RowBegin, 1, 3), Tuple.TupleSelect(hv_RowBegin, _
                    0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_ColBegin, 1, 3), Tuple.TupleSelect( _
                    hv_ColBegin, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange(hv_RowEnd, 1, _
                    3), Tuple.TupleSelect(hv_RowEnd, 0)), Tuple.TupleConcat(Tuple.TupleSelectRange( _
                    hv_ColEnd, 1, 3), Tuple.TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                    hv_IsParallel)
                'tuple_first_n (Rows, |Rows|-2, Rows)
                'tuple_first_n (Columns, |Columns|-2, Columns)

                '頂点１を抽出
                'まず、頂点付近の孔を調べる。
                Marshal.ReleaseComObject(ho_RegionIntersection1)
                Op.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                Marshal.ReleaseComObject(ho_RegionDifference)
                Op.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                Marshal.ReleaseComObject(ho_ConnectedRegions4)
                Op.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                Op.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                Op.TupleSort(hv_Area, hv_Sorted)
                Op.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                Op.TupleSelect(hv_Row, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                Op.TupleSelect(hv_Column, Tuple.TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                '次に、その孔から一番近い頂点を頂点１とする。
                Op.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                Op.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                Op.TuplePow(hv_DiffR, 2, hv_PowR)
                Op.TuplePow(hv_DiffC, 2, hv_PowC)
                Op.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                Op.TupleSqrt(hv_Sum, hv_Distance)
                Op.TupleSortIndex(hv_Distance, hv_Indices)
                hv_tempIndex = Tuple.TupleSelect(hv_Indices, 0)
                hv_C0_Row = Tuple.TupleSelect(hv_Rows, hv_tempIndex)
                hv_C0_Col = Tuple.TupleSelect(hv_Columns, hv_tempIndex)
                'その次に、残りの３頂点を特定する。
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Columns
                SP_C = SP_C + 1
                Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
                hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
                hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
                hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
                hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
                hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = Tuple.TupleSelect(hv_Rows, hv_I1)
                hv_C1_Col = Tuple.TupleSelect(hv_Columns, hv_I1)
                hv_C2_Row = Tuple.TupleSelect(hv_Rows, hv_I2)
                hv_C2_Col = Tuple.TupleSelect(hv_Columns, hv_I2)
                hv_C3_Row = Tuple.TupleSelect(hv_Rows, hv_I3)
                hv_C3_Col = Tuple.TupleSelect(hv_Columns, hv_I3)
                hv_OutRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_OutCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)

                '枠の4頂点で射影変換
                hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550, 900), _
                    550), 900)
                If CTargetType = 0 Then
                    hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550, 900), _
                        900), 550)
                Else
                    hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(550 + 7.855, 900), _
                      900 + 7.855), 550)
                End If

                Op.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                    System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, _
                    System.DBNull.Value, System.DBNull.Value, hv_HomMat2D, hv_Covariance)
                'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                '枠内の4点を抽出
                Marshal.ReleaseComObject(ho_RegionDifference1)
                Op.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                Op.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                Op.TupleMean(hv_Area, hv_MeanArea)
                Marshal.ReleaseComObject(ho_InTarget)
                Op.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 15, hv_MeanArea)
                Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                'Marshal.ReleaseComObject(ho_tempObj)
                'Op.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                'Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                If Tuple.TupleLength(hv_InRow) <> 4 Then
                    Continue For
                End If
                'Op.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                '枠内の４点の配置を調べる
                Op.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                    hv_ColTrans)
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                SP_C = 0
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                Op.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                SP_C = 0
                Op.TupleRound(hv_RowTrans, hv_RowIndex)
                Op.TupleRound(hv_ColTrans, hv_ColIndex)
                CTemp(SP_C) = hv_RowIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                SP_C = 0
                CTemp(SP_C) = hv_ColIndex
                SP_C = SP_C + 1
                Op.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                SP_C = 0
                hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(5, Tuple.TupleSub(hv_RowIndex, _
                    1)), hv_ColIndex)
                Op.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                CTemp(SP_C) = hv_CT_Number
                SP_C = SP_C + 1
                Op.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                SP_C = 0
                hv_strCT_Number = Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleAdd( _
                    Tuple.TupleAdd(Tuple.TupleAdd(Tuple.TupleSelect(hv_CT_Number, 0), "_"), _
                    Tuple.TupleSelect(hv_CT_Number, 1)), "_"), Tuple.TupleSelect(hv_CT_Number, _
                    2)), "_"), Tuple.TupleSelect(hv_CT_Number, 3))
                '配置によって、CTの番号を特定する。
                Op.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                If hv_CT_ID_Index = -1 Then
                    ' Stop
                    Continue For
                End If
                hv_CT_ID = Tuple.TupleAdd(hv_CT_ID_Index, 1)
                If hv_CT_ID = 47 Then
                    ' Continue For
                End If
                hv_CTRow = Tuple.TupleSelect(hv_InRow, hv_CT_NumberIndex)
                hv_CTCol = Tuple.TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                '  Op.ConcatObj(objTargetRegion, ho_InTarget, objTargetRegion) 'SUURI ADD 20150404
                If hv_CT_ID < 419 Then


                    Dim NewCT As New CodedTarget
                    NewCT.CT_ID = CInt(hv_CT_ID)
                    NewCT.ImageID1 = ImageID
                    NewCT.CT_Points.Row = hv_CTRow
                    NewCT.CT_Points.Col = hv_CTCol
                    NewCT.AllST_Area = Tuple.TupleSum(hv_AreaST) 'SUURI ADD 20150217
                    NewCT.SetlstCTtoST()
                    Dim BadCT As Boolean = False
                    For Each CT As CodedTarget In lstCT
                        If CT.CT_ID = NewCT.CT_ID Then
                            BadCT = True
                            Exit For

                        End If
                    Next
                    If BadCT = False Then
                        lstCT.Add(NewCT)
                        All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                        All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)

                    End If
                    Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                    SP_O = SP_O + 1
                    Marshal.ReleaseComObject(ho_CT_Regions)
                    Op.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                    Marshal.ReleaseComObject(OTemp(SP_O - 1))
                    SP_O = 0
                End If
            End If

        Next
        Op.CountObj(ho_CT_Regions, hv_ST_num)
        Op.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)

        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_CT_Regions)
        Op.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0

        'Marshal.ReleaseComObject(ho_Region)
        'Op.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        'Marshal.ReleaseComObject(ho_ConnectedRegions)
        'Op.Connection(ho_Region, ho_ConnectedRegions)

        Marshal.ReleaseComObject(ho_Difference)
        Op.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
        Op.FillUp(ho_Difference, ho_Difference) 'SUSANO ADD 20150917 
        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Dim param1 As Object = Nothing 'タプル結合だけしており、処理の無駄
        Dim parammin As Object = Nothing
        Dim parammax As Object = Nothing
        param1 = Tuple.TupleConcat(Tuple.TupleConcat("holes_num", "circularity"), "area")
        parammin = Tuple.TupleConcat(Tuple.TupleConcat(0, 0.3), 10.0)
        parammax = Tuple.TupleConcat(Tuple.TupleConcat(0, 1.0), 999999.0)

        Op.SelectShape(ho_Difference, ho_SingleTargetRegion, "holes_num", "and", 0, 0)
        Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "circularity", "and", 0.3, 1.0)
        'Dim minAreaFourTen As Integer = CInt(ReadFourTenTarget(6))
        'Dim maxAreaFourTen As Integer = CInt(ReadFourTenTarget(7))
        '20170719 kiryu Debug 999.0→99999.0 室内実験用
        Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", TGDetectPram.minAreaFourTen, TGDetectPram.maxAreaFourTen) ' SUSANO UPDATE 20151018 99999.0->999.0に変更

        '   Op.SelectShape(ho_Difference, ho_SingleTargetRegion, param1, "and", parammin, parammax)

        'Op.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '              Tuple.TupleConcat(Tuple.TupleConcat( _
        '               "holes_num", "circularity"), "area"), _
        '               "and", _
        '               Tuple.TupleConcat(Tuple.TupleConcat( _
        '               0, 0.3), 10), _
        '               Tuple.TupleConcat(Tuple.TupleConcat( _
        '               0, 1.0), 999999))
        'Op.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '             Tuple.TupleConcat("holes_num", "area"), _
        '              "and", Tuple.TupleConcat(0, 10), Tuple.TupleConcat(0, 999999))

        'VBMコードターゲットを除外する



        '**********************************
#If True Then   '4点TG認識処理





        Dim ho_RegionDilation As HUntypedObjectX = Nothing
        Dim ho_RegionUnion As HUntypedObjectX = Nothing
        Dim ho_ConnectedRegions5 As HUntypedObjectX = Nothing
        Dim ho_RegionIntersection2 As HUntypedObjectX = Nothing
        Dim ho_NonVBMCTRegion As HUntypedObjectX = Nothing
        Dim ho_objFourTenTarget As HUntypedObjectX = Nothing, ho_objConnectionFourTenTarget As HUntypedObjectX = Nothing
        Dim ho_SelectedRegions1 As HUntypedObjectX = Nothing, ho_SelectedRegions3 As HUntypedObjectX = Nothing


        Dim objFourTenTargetCandidate As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate1 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate2 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate3 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate4 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate12 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate34 As HUntypedObjectX = Nothing
        Dim objFourTenTargetCandidate1234 As HUntypedObjectX = Nothing


        Dim hv_FourTen_num As Object = Nothing, hv_Index1 As Object = Nothing
        Dim hv_FourTenCandidate_num As Object = Nothing
        Dim hv_Number1 As Object = Nothing
        Dim hv_FourTenComb_num As Object = Nothing
        Dim TenNum As Object = Nothing



        Op.GenEmptyObj(ho_RegionDilation)
        Marshal.ReleaseComObject(ho_RegionDilation)
        Op.GenEmptyObj(ho_RegionUnion)
        Marshal.ReleaseComObject(ho_RegionUnion)
        Op.GenEmptyObj(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Op.GenEmptyObj(ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_RegionIntersection2)
        Op.GenEmptyObj(ho_NonVBMCTRegion)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)
        'SUSANO ADD START 20151018
        ' Op.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", 1.0, 500)
        'SUSANO ADD END 20151018


        '        dilation_seq (SingleTargetRegion, RegionDilation, 'c', 3)
        '        union1(RegionDilation, RegionUnion)
        '        connection(RegionUnion, ConnectedRegions5)
        '        intersection(ConnectedRegions5, SingleTargetRegion, RegionIntersection2)
        'select_shape (RegionIntersection2, NonVBMCTRegion, 'connect_num', 'and', 4, 5)
        '        count_obj(NonVBMCTRegion, FourTenCandidate_num)

        'Dim STConnectionRimitNum As Integer = 6
        'Dim dilationRadius As Integer = 3
        'dilationRadius = CInt(ReadFourTenTarget(5))
        Op.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", TGDetectPram.dilationRadius)   '20170716 Kiryu 3→9　室内実験用
        Op.Union1(ho_RegionDilation, ho_RegionUnion)
        Op.Connection(ho_RegionUnion, ho_ConnectedRegions5)
        Op.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
        Op.SelectShape(ho_RegionIntersection2, ho_NonVBMCTRegion, "connect_num", "and", 4, TGDetectPram.STConnectionRimit)

        Op.CountObj(ho_NonVBMCTRegion, hv_FourTen_num)
        Op.CountObj(ho_NonVBMCTRegion, hv_FourTenCandidate_num)


        'for Index2 := 1 to FourTenCandidate_num by 1
        '            select_obj(NonVBMCTRegion, objFourTenTargetCandidate, Index2)
        '            count_obj(NonVBMCTRegion, FourTenComb_num)

        '            connection(objFourTenTargetCandidate, objFourTenTargetCandidate)
        '            count_obj(objFourTenTargetCandidate, TenNum)

        'Combination(Nothing, 5, 4)

        'Dim win As New TrgtDtctDebugWin

        'If ImageID = 5 Then
        '    win.Show()
        '    win.SetTitle(ImageID, T_Threshold)
        '    win.Disp(ho_Image, Nothing)
        'End If

        'win.DispReg(ho_NonVBMCTRegion)


        For Index2 As Integer = 1 To hv_FourTenCandidate_num Step 1
            Debug.Print("Index2 = " & Index2)

            Op.SelectObj(ho_NonVBMCTRegion, objFourTenTargetCandidate, Index2)
            Op.CountObj(ho_NonVBMCTRegion, hv_FourTenComb_num)


            Op.Connection(objFourTenTargetCandidate, objFourTenTargetCandidate)
            Op.CountObj(objFourTenTargetCandidate, TenNum)

            '    if ( TenNum == 4)
            '        IndexCmb:= [[1,2,3,4]]
            '        roopNum := 0
            '    elseif(TenNum == 5)
            '        IndexCmb:= [[1,2,3,4],[1,2,3,5],[1,2,4,5],[1,3,4,5],[2,3,4,5]]
            '        roopNum := 4
            '            End If


            '*for IndexX := 1 to FourTenComb_num by 1
            '    for ssnum := 0 to roopNum  by 1
            '        idx1 := IndexCmb[ssnum*4+0]
            '        idx2 := IndexCmb[ssnum*4+1]
            '        idx3 := IndexCmb[ssnum*4+2]
            '        idx4 := IndexCmb[ssnum*4+3]
            '        select_obj(objFourTenTargetCandidate, objFourTenTargetCandidate1, idx1)
            '        select_obj(objFourTenTargetCandidate, objFourTenTargetCandidate2, idx2)
            '        select_obj(objFourTenTargetCandidate, objFourTenTargetCandidate3, idx3)
            '        select_obj(objFourTenTargetCandidate, objFourTenTargetCandidate4, idx4)
            '        concat_obj(objFourTenTargetCandidate1, objFourTenTargetCandidate2, objFourTenTargetCandidate12)
            '        concat_obj(objFourTenTargetCandidate3, objFourTenTargetCandidate4, objFourTenTargetCandidate34)
            '        concat_obj(objFourTenTargetCandidate12, objFourTenTargetCandidate34, objFourTenTargetCandidate1234)

            '        count_obj(NonVBMCTRegion, FourTen_num)




            '********************************************************
            '4点以上の組み合わせを全て、パターンマッチング
            '********************************************************
            Dim list = New List(Of String)
            Dim CombList = New List(Of Integer())

            Dim cnt As Integer = 0
            makeCombination(list, TenNum, 4, 0, 0, CombList, cnt)
            'makeCombination(list, 4, 4, 0, 0, CombList, cnt)

            For ssnum As Integer = 0 To cnt - 1 Step 1
                Debug.Print("ssnum = " & ssnum)
                Op.SelectObj(objFourTenTargetCandidate, objFourTenTargetCandidate1, CombList(ssnum)(0) + 1)
                Op.SelectObj(objFourTenTargetCandidate, objFourTenTargetCandidate2, CombList(ssnum)(1) + 1)
                Op.SelectObj(objFourTenTargetCandidate, objFourTenTargetCandidate3, CombList(ssnum)(2) + 1)
                Op.SelectObj(objFourTenTargetCandidate, objFourTenTargetCandidate4, CombList(ssnum)(3) + 1)
                Op.ConcatObj(objFourTenTargetCandidate1, objFourTenTargetCandidate2, objFourTenTargetCandidate12)
                Op.ConcatObj(objFourTenTargetCandidate3, objFourTenTargetCandidate4, objFourTenTargetCandidate34)
                Op.ConcatObj(objFourTenTargetCandidate12, objFourTenTargetCandidate34, objFourTenTargetCandidate1234)
                Op.CountObj(ho_NonVBMCTRegion, hv_FourTen_num)

                'win.Disp(ho_Image, Nothing)
                'win.DispReg(objFourTenTargetCandidate1234)


                '**ターゲットらしきものの判定

                '    *for Index1 := 1 to FourTen_num by 1

                '        *select_obj (NonVBMCTRegion, objFourTenTarget, Index1)
                '        *connection (objFourTenTarget,objConnectionFourTenTarget)
                '        select_shape (objFourTenTargetCandidate1234, SelectedRegions1, 'area', 'and', 10, 99999)
                '        select_shape (SelectedRegions1, SelectedRegions3, 'circularity', 'and', 0.7, 1)
                '        count_obj(SelectedRegions3, Number1)


                ' For hv_Index1 = 1 To hv_FourTen_num Step 1

                'Call Op.SelectObj(ho_NonVBMCTRegion, ho_objFourTenTarget, Index2)
                'Call Op.Connection(ho_objFourTenTarget, ho_objConnectionFourTenTarget)

                'Call Op.SelectShape(ho_objConnectionFourTenTarget, ho_SelectedRegions1, "area", _
                '    "and", TGDetectPram.minAreaFourTen, TGDetectPram.maxAreaFourTen)

                Call Op.SelectShape(objFourTenTargetCandidate1234, ho_SelectedRegions1, "area", _
    "and", TGDetectPram.minAreaFourTen, TGDetectPram.maxAreaFourTen)


                '真円度0.7以上
                Call Op.SelectShape(ho_SelectedRegions1, ho_SelectedRegions3, "circularity", _
                    "and", TGDetectPram.circularity, 1)

                Call Op.CountObj(ho_SelectedRegions3, hv_Number1)

                If Tuple.TupleEqual(hv_Number1, 4) Then

                    Get2DCoord(ho_ImageChannel1, ho_SelectedRegions3, hv_CTRow, hv_CTCol, hv_AreaST)

                    Dim hvAreaSort As Object = Nothing
                    hvAreaSort = Tuple.TupleSortIndex(hv_AreaST)
                    '20150914 UPDATE START SUSANO 4tenTargetの大３点の面積の比率を考慮する。
                    Dim hvArea012 As Object = Nothing
                    Dim hvArea012s As Object = Nothing
                    hvArea012s = Tuple.TupleConcat(Tuple.TupleConcat(hv_AreaST(hvAreaSort(1)), hv_AreaST(hvAreaSort(2))), hv_AreaST(hvAreaSort(3)))
                    hvArea012 = Tuple.TupleMean(hvArea012s)
                    Dim hvArea012_dev As Object = Nothing
                    hvArea012_dev = Tuple.TupleDeviation(hvArea012s)

                    Dim hvArea3 As Object = Nothing
                    hvArea3 = hv_AreaST(hvAreaSort(0))
                    Debug.Print("camID=" & ImageID & "TT=" & T_Threshold & " 平均面積=" & hvArea012 & "," & hvArea012_dev & vbNewLine)
                    My.Computer.FileSystem.WriteAllText("4tenTargetMonitor.txt", "camID=" & ImageID & " 平均面積=" & hvArea012 & "," & hvArea012_dev & vbNewLine, True)
                    'Dim dblsetAreaHiritu As Double = CDbl(ReadFourTenTarget(1))
                    'Dim dblsetArea012 As Double = CDbl(ReadFourTenTarget(2))
                    'Dim dblsetArea012_dev As Double = CDbl(ReadFourTenTarget(3))
                    If hvArea012 * TGDetectPram.dblsetAreaHiritu > hvArea3 And hvArea012 > TGDetectPram.dblsetArea012 And hvArea012_dev < TGDetectPram.dblsetArea012_dev Then
                        '20150915 UPDATE END SUSANO 4tenTargetの大３点の面積の比率を考慮する。

                        Dim R0 As Object = Nothing
                        Dim C0 As Object = Nothing
                        Dim R1 As Object = Nothing
                        Dim C1 As Object = Nothing
                        Dim R2 As Object = Nothing
                        Dim C2 As Object = Nothing
                        Dim R3 As Object = Nothing
                        Dim C3 As Object = Nothing

                        If FourTenTarget1234_ver2(hv_CTRow, hv_CTCol, hvAreaSort(0), R0, C0, R1, C1, R2, C2, R3, C3) = -1 Then
                            Continue For
                        End If

                        'FourTenTarget1234(hv_CTRow, hv_CTCol, hvAreaSort(0), R0, C0, R1, C1, R2, C2, R3, C3)

                        hv_CTRow = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(R0, R1), R2), R3)
                        hv_CTCol = Tuple.TupleConcat(Tuple.TupleConcat(Tuple.TupleConcat(C0, C1), C2), C3)

                        hv_OutRow = Tuple.TupleConcat(Tuple.TupleConcat(R0, R1), R2)
                        hv_OutCol = Tuple.TupleConcat(Tuple.TupleConcat(C0, C1), C2)

                        '大3点で射影変換
                        hv_SRow = Tuple.TupleConcat(Tuple.TupleConcat(0, 0), 400)
                        hv_SCol = Tuple.TupleConcat(Tuple.TupleConcat(0, 400), 0)
                        Op.VectorToHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, hv_HomMat2D)

                        Op.AffineTransPoint2D(hv_HomMat2D, R3, C3, hv_RowTrans, hv_ColTrans)
                        'R3 = hv_RowTrans(3) - 280
                        'C3 = hv_ColTrans(3) - 280

                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        Op.TupleSub(CTemp(SP_C - 1), 280, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        Op.TupleSub(CTemp(SP_C - 1), 280, hv_ColTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_RowTrans
                        SP_C = SP_C + 1
                        Op.TupleDiv(CTemp(SP_C - 1), 120, hv_RowTrans)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColTrans
                        SP_C = SP_C + 1
                        Op.TupleDiv(CTemp(SP_C - 1), 120, hv_ColTrans)
                        SP_C = 0
                        Op.TupleRound(hv_RowTrans, hv_RowIndex)
                        Op.TupleRound(hv_ColTrans, hv_ColIndex)
                        CTemp(SP_C) = hv_RowIndex
                        SP_C = SP_C + 1
                        Op.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                        SP_C = 0
                        CTemp(SP_C) = hv_ColIndex
                        SP_C = SP_C + 1
                        Op.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                        SP_C = 0
                        'If hv_RowIndex = 0 Or hv_ColIndex = 0 Then
                        '    If hv_RowIndex = 0 Then
                        '        hv_CT_Number = 17
                        '    End If
                        '    If hv_ColIndex = 0 Then
                        '        hv_CT_Number = 18
                        '    End If
                        'Else
                        '    hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(4, Tuple.TupleSub(hv_RowIndex, _
                        '  1)), hv_ColIndex)
                        'End If
                        'If hv_RowIndex = 0 And hv_ColIndex = 5 Then
                        '    hv_CT_Number = 17
                        'ElseIf hv_RowIndex = 5 And hv_ColIndex = 0 Then
                        '    hv_CT_Number = 18
                        'Else
                        '    hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(4, Tuple.TupleSub(hv_RowIndex, _
                        '  1)), hv_ColIndex)
                        'End If
                        If hv_RowIndex = 0 Or hv_ColIndex = 0 Then
                            If hv_RowIndex = 0 Then
                                hv_CT_Number = 17
                            End If
                            If hv_ColIndex = 0 Then
                                hv_CT_Number = 18
                            End If
                        Else
                            hv_CT_Number = Tuple.TupleAdd(Tuple.TupleMult(4, Tuple.TupleSub(hv_RowIndex, _
                          1)), hv_ColIndex)
                        End If



                        Op.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                        CTemp(SP_C) = hv_CT_Number
                        SP_C = SP_C + 1
                        Op.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                        SP_C = 0
                        hv_strCT_Number = hv_CT_Number
                        If ImageID = 11 And hv_CT_Number = 11 Then
                            Debug.Print("da")
                        End If
                        If hv_CT_Number < 19 And hv_CT_Number > 0 Then
                            hv_strCT_Number = hv_CT_Number

                            Dim NewCT As New CodedTarget
                            NewCT.CT_ID = CInt(hv_CT_Number) + 400
                            NewCT.ImageID1 = ImageID
                            NewCT.CT_Points.Row = hv_CTRow
                            NewCT.CT_Points.Col = hv_CTCol
                            NewCT.AllST_Area = Tuple.TupleSum(hv_AreaST) 'SUURI ADD 20150217
                            NewCT.SetlstCTtoST()
                            lstCT.Add(NewCT)
                            Debug.Print(NewCT.CT_ID)
                            All2D.Row = Tuple.TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                            All2D.Col = Tuple.TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                        End If
                    End If
                End If
            Next

        Next

        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)
#End If
        '   Op.CountObj(ho_SingleTargetRegion, hv_ST_num)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        ' Marshal.ReleaseComObject(ho_tempObj)
        '  Op.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
        ' Op.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)
        Op.WriteRegion(ho_SingleTargetRegion, "SingleTargetRegion" & ImageID & ".reg")
        '  Op.ConcatObj(objTargetRegion, ho_SingleTargetRegion, objTargetRegion) 'SUURI ADD 20150404

        hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
        '  Op.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D.Row = Tuple.TupleConcat(All2D.Row, hv_ST_Rows)
        All2D.Col = Tuple.TupleConcat(All2D.Col, hv_ST_Cols)

        '基準点ターゲットを大白○だけで認識
        Dim intHanteiKyori As Integer = 10

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt")
        If IsNumeric(fileContents.Split(" ")(1)) Then
            intHanteiKyori = CInt(fileContents.Split(" ")(1))
        End If
        Dim dblsetAreaBigST As Double = CDbl(ReadFourTenTarget(4))
        For Each objtmpTT As CodedTarget In objBeforeTarget.lstCT
            If ImageID = 7 And objtmpTT.CT_ID = 195 Then
                Debug.Print("stop")
            End If
            Dim objMinKK As Double = Double.MaxValue
            Dim objMinKK_ST As SingleTarget = Nothing
            hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
            For hv_ind = 0 To Tuple.TupleSub(hv_ST_num, 1) Step 1
                Op.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
                Op.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
                Dim NewST As New SingleTarget
                NewST.P2D.Row = hv_ST_Row
                NewST.P2D.Col = hv_ST_Col
                NewST.P2ID = hv_ind + 1
                NewST.ImageID = ImageID
                NewST.flgUsed = 0
                Op.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
                '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217

                Dim tmpKK As Object = Nothing
                objtmpTT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(hv_ST_Row, hv_ST_Col, tmpKK)
                If objMinKK > tmpKK Then
                    objMinKK = tmpKK
                    objMinKK_ST = NewST
                End If
            Next

            If objMinKK < intHanteiKyori And objMinKK_ST.AreaST >= TGDetectPram.dblsetAreaBigST Then
                'Op.TupleRemove(hv_ST_Rows, objMinKK_ST.P2ID - 1, hv_ST_Rows)
                'Op.TupleRemove(hv_ST_Cols, objMinKK_ST.P2ID - 1, hv_ST_Cols)
                Dim NewCT As New CodedTarget
                NewCT.CT_ID = objtmpTT.CT_ID
                NewCT.ImageID1 = objtmpTT.ImageID1
                'Dim oneSTtoCT As New ImagePoints
                'oneSTtoCT.Row = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                'oneSTtoCT.Col = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))

                NewCT.CT_Points.Row = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), Tuple.TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                NewCT.CT_Points.Col = Tuple.TupleConcat(Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), Tuple.TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))
                NewCT.AllST_Area = objMinKK_ST.AreaST
                NewCT.SetlstCTtoST()
                lstCT.Add(NewCT)
            End If
        Next

        hv_ST_num = Tuple.TupleLength(hv_ST_Rows)
        For hv_ind = 0 To Tuple.TupleSub(hv_ST_num, 1) Step 1
            Op.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
            Op.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
            Dim NewST As New SingleTarget
            NewST.P2D.Row = hv_ST_Row
            NewST.P2D.Col = hv_ST_Col
            NewST.P2ID = hv_ind + 1
            NewST.ImageID = ImageID
            NewST.flgUsed = 0
            Op.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
            '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217
            lstST.Add(NewST)
        Next

        'SUURI ADD 20150217
#If True Then
        insertSTtoCTpoint()

        'For Each objCT As CodedTarget In lstCT
        '    Dim mindistST As Double = Double.MaxValue
        '    Dim minST As SingleTarget = Nothing
        '    '対象のＣＴに一番近いＳＴを取得
        '    For Each objST As SingleTarget In lstST
        '        Dim objKyori As Object = Nothing
        '        'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
        '        'つまり、ある程度大きさのあるＳＴを選択する。
        '        If objST.AreaST > objCT.AllST_Area Then
        '            objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
        '                                                             objST.P2D.Col,
        '                                                             objKyori)
        '            If objKyori < mindistST Then
        '                mindistST = objKyori
        '                minST = objST
        '            End If
        '        End If
        '    Next
        '    '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
        '    'つまりある程度CTに近いＳＴを選択する。
        '    If mindistST < objCT.GetAllPointsKyoriPixel Then

        '        objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
        '        objCT.CT_Points.Row(0) = minST.P2D.Row
        '        objCT.CT_Points.Col(0) = minST.P2D.Col
        '        objCT.CenterPoint.CopyToMe(minST.P2D)

        '    End If
        'Next

#End If
        'SUURI ADD 20150217 
        All2D_ST.Row = hv_ST_Rows
        All2D_ST.Col = hv_ST_Cols

        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Marshal.ReleaseComObject(ho_ObjectSelected)
        Marshal.ReleaseComObject(ho_ConnectedRegions3)
        Marshal.ReleaseComObject(ho_Waku)
        Marshal.ReleaseComObject(ho_FillWaku)
        Marshal.ReleaseComObject(ho_Contour)
        Marshal.ReleaseComObject(ho_RegressContours)
        Marshal.ReleaseComObject(ho_UnionContours)
        Marshal.ReleaseComObject(ho_ContoursSplit)
        Marshal.ReleaseComObject(ho_RegionIntersection1)
        Marshal.ReleaseComObject(ho_RegionDifference)
        Marshal.ReleaseComObject(ho_ConnectedRegions4)
        Marshal.ReleaseComObject(ho_RegionDifference1)
        Marshal.ReleaseComObject(ho_InTarget)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Marshal.ReleaseComObject(ho_SelectedRegionsT)

        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Marshal.ReleaseComObject(ho_Difference)
        Marshal.ReleaseComObject(ho_tempObj)
        Marshal.ReleaseComObject(ho_ContourT)

        ' win.Close()
    End Sub



#If False Then

    Public Sub DetectTargetsByHalconDotNet(ByVal ImageID As Integer,
                               ByVal ho_Image As HalconDotNet.HImage,
                               ByVal hv_CT_IDs As Object,
                               ByVal T_Threshold As Integer,
                               ByVal CTargetType As Integer)
        ' Stack for temporary control variables 
        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0

        ' Stack for temporary objects 
        Dim OTemp(10) As HalconDotNet.HObject
        Dim SP_O As Integer
        SP_O = 0
        Dim Op As New HALCONXLib.HOperatorSetX
        Dim Tuple As New HALCONXLib.HTupleX
        ' Local iconic variables 
        Dim ho_Region As HalconDotNet.HObject = Nothing, ho_ConnectedRegions As HalconDotNet.HObject = Nothing
        Dim ho_SelectedRegions As HalconDotNet.HObject = Nothing
        Dim ho_RegionFillUp As HalconDotNet.HObject = Nothing, ho_RegionIntersection As HalconDotNet.HObject = Nothing
        Dim ho_SelectedRegions2 As HalconDotNet.HObject = Nothing
        Dim ho_ConnectedRegions1 As HalconDotNet.HObject = Nothing
        Dim ho_ConnectedRegions2 As HalconDotNet.HObject = Nothing
        Dim ho_ObjectSelected As HalconDotNet.HObject = Nothing
        Dim ho_ConnectedRegions3 As HalconDotNet.HObject = Nothing
        Dim ho_Waku As HalconDotNet.HObject = Nothing, ho_FillWaku As HalconDotNet.HObject = Nothing
        Dim ho_Contour As HalconDotNet.HObject = Nothing, ho_RegressContours As HalconDotNet.HObject = Nothing
        Dim ho_UnionContours As HalconDotNet.HObject = Nothing, ho_ContoursSplit As HalconDotNet.HObject = Nothing
        Dim ho_RegionIntersection1 As HalconDotNet.HObject = Nothing
        Dim ho_RegionDifference As HalconDotNet.HObject = Nothing
        Dim ho_ConnectedRegions4 As HalconDotNet.HObject = Nothing
        Dim ho_TransRegions As HalconDotNet.HObject = Nothing
        Dim ho_RegionDifference1 As HalconDotNet.HObject = Nothing
        Dim ho_InTarget As HalconDotNet.HObject = Nothing
        Dim ho_ObjectSelected1 As HalconDotNet.HObject = Nothing
        Dim ho_SelectedRegionsT As HalconDotNet.HObject = Nothing
        Dim ho_SingleTargetRegion As HalconDotNet.HObject = Nothing
        Dim ho_Difference As HalconDotNet.HObject = Nothing
        Dim ho_CT_Regions As HalconDotNet.HObject = Nothing
        Dim ho_tempObj As HalconDotNet.HObject = Nothing
        Dim ho_ImageChannel1 As HalconDotNet.HObject = Nothing
        Dim ho_ContourT As HalconDotNet.HObject = Nothing

        ' Local control variables 
        Dim hv_WindowHandle As HalconDotNet.HTuple = Nothing
        Dim hv_Number As HalconDotNet.HTuple = Nothing, hv_Index As HalconDotNet.HTuple = Nothing
        Dim hv_objNum As HalconDotNet.HTuple = Nothing, hv_Rows As HalconDotNet.HTuple = Nothing
        Dim hv_Columns As HalconDotNet.HTuple = Nothing, hv_RowBegin As HalconDotNet.HTuple = Nothing
        Dim hv_ColBegin As HalconDotNet.HTuple = Nothing, hv_RowEnd As HalconDotNet.HTuple = Nothing
        Dim hv_ColEnd As HalconDotNet.HTuple = Nothing, hv_Nr As HalconDotNet.HTuple = Nothing
        Dim hv_Nc As HalconDotNet.HTuple = Nothing, hv_Dist As HalconDotNet.HTuple = Nothing
        Dim hv_IsParallel As HalconDotNet.HTuple = Nothing, hv_Area As HalconDotNet.HTuple = Nothing
        Dim hv_Row As HalconDotNet.HTuple = Nothing, hv_Column As HalconDotNet.HTuple = Nothing
        Dim hv_Sorted As HalconDotNet.HTuple = Nothing, hv_SortedAreaIndex As HalconDotNet.HTuple = Nothing
        Dim hv_AnaRow As HalconDotNet.HTuple = Nothing, hv_AnaColumn As HalconDotNet.HTuple = Nothing
        Dim hv_DiffR As HalconDotNet.HTuple = Nothing, hv_DiffC As HalconDotNet.HTuple = Nothing
        Dim hv_PowR As HalconDotNet.HTuple = Nothing, hv_PowC As HalconDotNet.HTuple = Nothing
        Dim hv_Sum As HalconDotNet.HTuple = Nothing, hv_Distance As HalconDotNet.HTuple = Nothing
        Dim hv_Indices As HalconDotNet.HTuple = Nothing, hv_tempIndex As HalconDotNet.HTuple = Nothing
        Dim hv_C0_Row As HalconDotNet.HTuple = Nothing, hv_C0_Col As HalconDotNet.HTuple = Nothing
        Dim hv_C1_Row As HalconDotNet.HTuple = Nothing, hv_C1_Col As HalconDotNet.HTuple = Nothing
        Dim hv_C2_Row As HalconDotNet.HTuple = Nothing, hv_C2_Col As HalconDotNet.HTuple = Nothing
        Dim hv_C3_Row As HalconDotNet.HTuple = Nothing, hv_C3_Col As HalconDotNet.HTuple = Nothing
        Dim hv_blnCheck As HalconDotNet.HTuple = Nothing, hv_Vec1_R As HalconDotNet.HTuple = Nothing
        Dim hv_Vec1_C As HalconDotNet.HTuple = Nothing, hv_Vec2_R As HalconDotNet.HTuple = Nothing
        Dim hv_Vec2_C As HalconDotNet.HTuple = Nothing, hv_Vec3_R As HalconDotNet.HTuple = Nothing
        Dim hv_Vec3_C As HalconDotNet.HTuple = Nothing, hv_Gaiseki1 As HalconDotNet.HTuple = Nothing
        Dim hv_Gaiseki2 As HalconDotNet.HTuple = Nothing, hv_Result As HalconDotNet.HTuple = Nothing
        Dim hv_I1 As HalconDotNet.HTuple = Nothing, hv_I2 As HalconDotNet.HTuple = Nothing
        Dim hv_I3 As HalconDotNet.HTuple = Nothing, hv_OutRow As HalconDotNet.HTuple = Nothing
        Dim hv_OutCol As HalconDotNet.HTuple = Nothing, hv_SRow As HalconDotNet.HTuple = Nothing
        Dim hv_SCol As HalconDotNet.HTuple = Nothing, hv_HomMat2D As HalconDotNet.HTuple = Nothing
        Dim hv_Covariance As HalconDotNet.HTuple = Nothing, hv_MeanArea As HalconDotNet.HTuple = Nothing
        Dim hv_InArea As HalconDotNet.HTuple = Nothing, hv_InRow As HalconDotNet.HTuple = Nothing
        Dim hv_InColumn As HalconDotNet.HTuple = Nothing, hv_RowTrans As HalconDotNet.HTuple = Nothing
        Dim hv_ColTrans As HalconDotNet.HTuple = Nothing, hv_RowIndex As HalconDotNet.HTuple = Nothing
        Dim hv_ColIndex As HalconDotNet.HTuple = Nothing, hv_CT_Number As HalconDotNet.HTuple = Nothing
        Dim hv_strCT_Number As HalconDotNet.HTuple = Nothing, hv_CT_ID_Index As HalconDotNet.HTuple = Nothing
        Dim hv_CT_ID As HalconDotNet.HTuple = Nothing, hv_Length As HalconDotNet.HTuple = Nothing
        Dim hv_Indices1 As HalconDotNet.HTuple = Nothing, hv_Selected As HalconDotNet.HTuple = Nothing
        Dim hv_Sorted1 As HalconDotNet.HTuple = Nothing, hv_CT_NumberIndex As HalconDotNet.HTuple = Nothing
        Dim hv_CTRow As HalconDotNet.HTuple = Nothing, hv_CTCol As HalconDotNet.HTuple = Nothing
        Dim hv_ST_num As HalconDotNet.HTuple = Nothing, hv_AreaST As HalconDotNet.HTuple = Nothing
        Dim hv_ind As HalconDotNet.HTuple = Nothing, hv_ST_Rows As HalconDotNet.HTuple = Nothing
        Dim hv_ST_Cols As HalconDotNet.HTuple = Nothing, hv_ST_Row As HalconDotNet.HTuple = Nothing
        Dim hv_ST_Col As HalconDotNet.HTuple = Nothing
        Dim hv_Length1 As HalconDotNet.HTuple = Nothing
        Dim hv_Indices2 As HalconDotNet.HTuple = Nothing
        Dim hv_Inverted As HalconDotNet.HTuple = Nothing
        Dim hv_Selected1 As HalconDotNet.HTuple = Nothing
        Dim hv_Sum1 As HalconDotNet.HTuple = Nothing

        HalconDotNet.HOperatorSet.GenEmptyObj(ho_Region)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_SelectedRegions)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionFillUp)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionIntersection)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_SelectedRegions2)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions1)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions2)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ObjectSelected)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions3)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_Waku)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_FillWaku)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_Contour)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegressContours)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_UnionContours)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ContoursSplit)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionIntersection1)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionDifference)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions4)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionDifference1)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_InTarget)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ObjectSelected1)

        HalconDotNet.HOperatorSet.GenEmptyObj(ho_CT_Regions)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_SelectedRegionsT)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_SingleTargetRegion)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_Difference)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_tempObj)

        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ImageChannel1)

        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ContourT)

        HalconDotNet.HOperatorSet.GenEmptyObj(objTargetRegion) 'SUURI ADD 20150405

        All2D.Row = DBNull.Value
        All2D.Col = DBNull.Value

        If lstCT Is Nothing Then
            lstCT = New List(Of CodedTarget)
        Else
            lstCT.Clear()
        End If

        'mean_image (Image, Image, 9, 9)

        ' HalconDotNet.HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)

        'HalconDotNet.HOperatorSet.VarThreshold(ho_Image, ho_Region, 10, 10, 0.2, T_Threshold, "light")
        'Marshal.ReleaseComObject(ho_tempObj)
        'HalconDotNet.HOperatorSet.BinThreshold(ho_Image, ho_tempObj)
        'Marshal.ReleaseComObject(ho_Region)
        'HalconDotNet.HOperatorSet.Complement(ho_tempObj, ho_Region)
        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ImageChannel1)
        GetFirstRegionG(ho_Image, ho_Region, ho_ImageChannel1, T_Threshold)
        'HalconDotNet.HOperatorSet.Threshold(ho_Image, ho_Region, 100, 255)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        HalconDotNet.HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        HalconDotNet.HOperatorSet.SelectShape(ho_ConnectedRegions, ho_SelectedRegions, "holes_num", "and", 2, 2)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        HalconDotNet.HOperatorSet.FillUp(ho_SelectedRegions, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        HalconDotNet.HOperatorSet.Intersection(ho_RegionFillUp, ho_ConnectedRegions, ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        HalconDotNet.HOperatorSet.SelectShape(ho_RegionIntersection, ho_SelectedRegions2, "connect_num", "and", 5, 5)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        HalconDotNet.HOperatorSet.FillUp(ho_SelectedRegions2, ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        HalconDotNet.HOperatorSet.Connection(ho_RegionFillUp, ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        HalconDotNet.HOperatorSet.Connection(ho_SelectedRegions2, ho_ConnectedRegions2)
        HalconDotNet.HOperatorSet.CountObj(ho_RegionFillUp, hv_Number)
        Marshal.ReleaseComObject(ho_CT_Regions)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_CT_Regions)

        For hv_Index = 1 To hv_Number Step 1
            Marshal.ReleaseComObject(ho_ObjectSelected)
            HalconDotNet.HOperatorSet.SelectObj(ho_SelectedRegions2, ho_ObjectSelected, hv_Index)
            Marshal.ReleaseComObject(ho_ConnectedRegions3)
            HalconDotNet.HOperatorSet.Connection(ho_ObjectSelected, ho_ConnectedRegions3)
            HalconDotNet.HOperatorSet.CountObj(ho_ConnectedRegions3, hv_objNum)
            If hv_objNum.TupleEqual(New HalconDotNet.HTuple(5)).I = 1 Then
                Marshal.ReleaseComObject(ho_Waku)
                HalconDotNet.HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_Waku, "holes_num", "and", 2, 2)
                Marshal.ReleaseComObject(ho_FillWaku)
                HalconDotNet.HOperatorSet.FillUp(ho_Waku, ho_FillWaku)
                '枠の４頂点を抽出
                '枠の外枠の4頂点ではなく内枠の4頂点を抽出するように変更　20120404
                Marshal.ReleaseComObject(ho_ContourT)
                HalconDotNet.HOperatorSet.GenContourRegionXld(ho_Waku, ho_ContourT, "border_holes")
                HalconDotNet.HOperatorSet.LengthXld(ho_ContourT, hv_Length1)
                HalconDotNet.HOperatorSet.TupleSortIndex(hv_Length1, hv_Indices2)
                HalconDotNet.HOperatorSet.TupleInverse(hv_Indices2, hv_Inverted)
                HalconDotNet.HOperatorSet.TupleSelect(hv_Inverted, 1, hv_Selected1)
                HalconDotNet.HOperatorSet.TupleAdd(hv_Selected1, 1, hv_Sum1)
                Marshal.ReleaseComObject(ho_Contour)
                HalconDotNet.HOperatorSet.SelectObj(ho_ContourT, ho_Contour, hv_Sum1)

                'get_region_polygon (FillWaku, 5, Rows, Columns)
                'gen_contour_polygon_xld (Contour, Rows, Columns)
                '必ず四角を見つけること！！！！！！！！
                'まだまだ
                Marshal.ReleaseComObject(ho_RegressContours)
                HalconDotNet.HOperatorSet.RegressContoursXld(ho_Contour, ho_RegressContours, "median", 1)
                Marshal.ReleaseComObject(ho_UnionContours)
                HalconDotNet.HOperatorSet.UnionCollinearContoursXld(ho_RegressContours, ho_UnionContours, 10, 2, 1, 0.1, "attr_keep")
                Marshal.ReleaseComObject(ho_ContoursSplit)
                HalconDotNet.HOperatorSet.SegmentContoursXld(ho_UnionContours, ho_ContoursSplit, "lines", 5, 4, 2)
                HalconDotNet.HOperatorSet.LengthXld(ho_ContoursSplit, hv_Length)
                HalconDotNet.HOperatorSet.TupleSortIndex(hv_Length, hv_Indices1)
                HalconDotNet.HOperatorSet.TupleLastN(hv_Indices1, (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleLength(hv_Indices1), 4), hv_Selected)
                HalconDotNet.HOperatorSet.TupleSort(hv_Selected, hv_Sorted1)
                Marshal.ReleaseComObject(ho_ObjectSelected1)

                Dim hv_Selected_L As HalconDotNet.HTuple = Nothing
                hv_Selected_L = (New HalconDotNet.HTuple).TupleAdd(hv_Sorted1, 1)

                Try

                    Dim i As Integer
                    HalconDotNet.HOperatorSet.GenEmptyObj(ho_ObjectSelected1)
                    For i = 1 To (New HalconDotNet.HTuple).TupleLength(hv_Selected_L)
                        Dim ho_tmpobj As HalconDotNet.HObject = Nothing
                        HalconDotNet.HOperatorSet.GenEmptyObj(ho_tmpobj)
                        HalconDotNet.HOperatorSet.SelectObj(ho_ContoursSplit, ho_tmpobj, hv_Selected_L(i - 1))
                        HalconDotNet.HOperatorSet.ConcatObj(ho_ObjectSelected1, ho_tmpobj, ho_ObjectSelected1)
                        Marshal.ReleaseComObject(ho_tmpobj)
                    Next


                Catch ex As Exception
                    Dim sss As String = ""
                    sss = ex.Message
                End Try


                HalconDotNet.HOperatorSet.FitLineContourXld(ho_ObjectSelected1, "tukey", -1, 0, 5, 2, hv_RowBegin, _
                    hv_ColBegin, hv_RowEnd, hv_ColEnd, hv_Nr, hv_Nc, hv_Dist)
                HalconDotNet.HOperatorSet.IntersectionLl(hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, (New HalconDotNet.HTuple).TupleConcat( _
                     (New HalconDotNet.HTuple).TupleSelectRange(hv_RowBegin, 1, 3), (New HalconDotNet.HTuple).TupleSelect(hv_RowBegin, _
                    0)), (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleSelectRange(hv_ColBegin, 1, 3), (New HalconDotNet.HTuple).TupleSelect( _
                    hv_ColBegin, 0)), (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleSelectRange(hv_RowEnd, 1, _
                    3), (New HalconDotNet.HTuple).TupleSelect(hv_RowEnd, 0)), (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleSelectRange( _
                    hv_ColEnd, 1, 3), (New HalconDotNet.HTuple).TupleSelect(hv_ColEnd, 0)), hv_Rows, hv_Columns, _
                    hv_IsParallel)
                'tuple_first_n (Rows, |Rows|-2, Rows)
                'tuple_first_n (Columns, |Columns|-2, Columns)

                '頂点１を抽出
                'まず、頂点付近の孔を調べる。
                Marshal.ReleaseComObject(ho_RegionIntersection1)
                HalconDotNet.HOperatorSet.Intersection(ho_Waku, ho_FillWaku, ho_RegionIntersection1)
                Marshal.ReleaseComObject(ho_RegionDifference)
                HalconDotNet.HOperatorSet.Difference(ho_FillWaku, ho_Waku, ho_RegionDifference)
                Marshal.ReleaseComObject(ho_ConnectedRegions4)
                HalconDotNet.HOperatorSet.Connection(ho_RegionDifference, ho_ConnectedRegions4)
                HalconDotNet.HOperatorSet.AreaCenter(ho_ConnectedRegions4, hv_Area, hv_Row, hv_Column)
                HalconDotNet.HOperatorSet.TupleSort(hv_Area, hv_Sorted)
                HalconDotNet.HOperatorSet.TupleSortIndex(hv_Area, hv_SortedAreaIndex)
                HalconDotNet.HOperatorSet.TupleSelect(hv_Row, (New HalconDotNet.HTuple).TupleSelect(hv_SortedAreaIndex, 0), hv_AnaRow)
                HalconDotNet.HOperatorSet.TupleSelect(hv_Column, (New HalconDotNet.HTuple).TupleSelect(hv_SortedAreaIndex, 0), hv_AnaColumn)

                '次に、その孔から一番近い頂点を頂点１とする。
                HalconDotNet.HOperatorSet.TupleSub(hv_Rows, hv_AnaRow, hv_DiffR)
                HalconDotNet.HOperatorSet.TupleSub(hv_Columns, hv_AnaColumn, hv_DiffC)
                HalconDotNet.HOperatorSet.TuplePow(hv_DiffR, 2, hv_PowR)
                HalconDotNet.HOperatorSet.TuplePow(hv_DiffC, 2, hv_PowC)
                HalconDotNet.HOperatorSet.TupleAdd(hv_PowR, hv_PowC, hv_Sum)
                HalconDotNet.HOperatorSet.TupleSqrt(hv_Sum, hv_Distance)
                HalconDotNet.HOperatorSet.TupleSortIndex(hv_Distance, hv_Indices)
                hv_tempIndex = (New HalconDotNet.HTuple).TupleSelect(hv_Indices, 0)
                hv_C0_Row = (New HalconDotNet.HTuple).TupleSelect(hv_Rows, hv_tempIndex)
                hv_C0_Col = (New HalconDotNet.HTuple).TupleSelect(hv_Columns, hv_tempIndex)
                'その次に、残りの３頂点を特定する。
                hv_C1_Row = 0
                hv_C1_Col = 0
                hv_C2_Row = 0
                hv_C2_Col = 0
                hv_C3_Row = 0
                hv_C3_Col = 0
                CTemp(SP_C) = hv_Rows
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
                SP_C = 0
                CTemp(SP_C) = hv_Columns
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
                SP_C = 0
                hv_blnCheck = -1
                hv_Vec1_R = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Rows, 0), hv_C0_Row)
                hv_Vec1_C = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Columns, 0), hv_C0_Col)
                hv_Vec2_R = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Rows, 1), hv_C0_Row)
                hv_Vec2_C = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Columns, 1), hv_C0_Col)
                hv_Vec3_R = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Rows, 2), hv_C0_Row)
                hv_Vec3_C = (New HalconDotNet.HTuple).TupleSub((New HalconDotNet.HTuple).TupleSelect(hv_Columns, 2), hv_C0_Col)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                hv_Result = (New HalconDotNet.HTuple).TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                If (New HalconDotNet.HTuple).TupleGreater(hv_Result, 0) = 1 Then
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                    CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
                    hv_Result = (New HalconDotNet.HTuple).TupleMult(hv_Gaiseki1, hv_Gaiseki2)
                    If (New HalconDotNet.HTuple).TupleGreater(hv_Result, 0) = 1 Then
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                        CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                        hv_I1 = 2
                        If (New HalconDotNet.HTuple).TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 1
                        Else
                            hv_I2 = 1
                            hv_I3 = 0
                        End If
                    Else
                        hv_I1 = 1
                        If (New HalconDotNet.HTuple).TupleLess(hv_Gaiseki1, 0) = 1 Then
                            hv_I2 = 0
                            hv_I3 = 2
                        Else
                            hv_I2 = 2
                            hv_I3 = 0
                        End If
                    End If
                Else
                    hv_I1 = 0
                    If (New HalconDotNet.HTuple).TupleLess(hv_Gaiseki1, 0) = 1 Then
                        hv_I2 = 1
                        hv_I3 = 2
                    Else
                        hv_I2 = 2
                        hv_I3 = 1
                    End If
                End If
                hv_C1_Row = (New HalconDotNet.HTuple).TupleSelect(hv_Rows, hv_I1)
                hv_C1_Col = (New HalconDotNet.HTuple).TupleSelect(hv_Columns, hv_I1)
                hv_C2_Row = (New HalconDotNet.HTuple).TupleSelect(hv_Rows, hv_I2)
                hv_C2_Col = (New HalconDotNet.HTuple).TupleSelect(hv_Columns, hv_I2)
                hv_C3_Row = (New HalconDotNet.HTuple).TupleSelect(hv_Rows, hv_I3)
                hv_C3_Col = (New HalconDotNet.HTuple).TupleSelect(hv_Columns, hv_I3)
                hv_OutRow = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(hv_C0_Row, _
                    hv_C1_Row), hv_C2_Row), hv_C3_Row)
                hv_OutCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(hv_C0_Col, _
                    hv_C1_Col), hv_C2_Col), hv_C3_Col)

                '枠の4頂点で射影変換
                hv_SRow = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(550, 900), _
                    550), 900)
                If CTargetType = 0 Then
                    hv_SCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(550, 900), _
                        900), 550)
                Else
                    hv_SCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(550 + 7.855, 900), _
                      900 + 7.855), 550)
                End If

                HalconDotNet.HOperatorSet.VectorToProjHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, "gold_standard", _
                    Nothing, Nothing, Nothing, Nothing, _
                    Nothing, Nothing, hv_HomMat2D, hv_Covariance)
                'projective_trans_region (ConnectedRegions3, TransRegions, HomMat2D, 'bilinear')
                '枠内の4点を抽出
                Marshal.ReleaseComObject(ho_RegionDifference1)
                HalconDotNet.HOperatorSet.Difference(ho_ConnectedRegions3, ho_Waku, ho_RegionDifference1)
                HalconDotNet.HOperatorSet.AreaCenter(ho_ConnectedRegions3, hv_Area, hv_Row, hv_Column)
                HalconDotNet.HOperatorSet.TupleMean(hv_Area, hv_MeanArea)
                Marshal.ReleaseComObject(ho_InTarget)
                HalconDotNet.HOperatorSet.SelectShape(ho_ConnectedRegions3, ho_InTarget, "area", "and", 15, hv_MeanArea)
                Get2DCoord(ho_ImageChannel1, ho_InTarget, hv_InRow, hv_InColumn, hv_AreaST)
                'Marshal.ReleaseComObject(ho_tempObj)
                'HalconDotNet.HOperatorSet.DilationCircle(ho_InTarget, ho_tempObj, 1.5)
                'HalconDotNet.HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_InArea, hv_InRow, hv_InColumn)
                If (New HalconDotNet.HTuple).TupleLength(hv_InRow) <> 4 Then
                    Continue For
                End If
                'HalconDotNet.HOperatorSet.AreaCenter(ho_InTarget, hv_InArea, hv_InRow, hv_InColumn)

                '枠内の４点の配置を調べる
                HalconDotNet.HOperatorSet.ProjectiveTransPixel(hv_HomMat2D, hv_InRow, hv_InColumn, hv_RowTrans, _
                    hv_ColTrans)
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleSub(CTemp(SP_C - 1), 625, hv_ColTrans)
                SP_C = 0
                CTemp(SP_C) = hv_RowTrans
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_RowTrans)
                SP_C = 0
                CTemp(SP_C) = hv_ColTrans
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleDiv(CTemp(SP_C - 1), 50, hv_ColTrans)
                SP_C = 0
                HalconDotNet.HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                HalconDotNet.HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                CTemp(SP_C) = hv_RowIndex
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                SP_C = 0
                CTemp(SP_C) = hv_ColIndex
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                SP_C = 0
                hv_CT_Number = (New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleMult(5, (New HalconDotNet.HTuple).TupleSub(hv_RowIndex, _
                    1)), hv_ColIndex)
                HalconDotNet.HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                CTemp(SP_C) = hv_CT_Number
                SP_C = SP_C + 1
                HalconDotNet.HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                SP_C = 0
                hv_strCT_Number = (New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleAdd( _
                     (New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleSelect(hv_CT_Number, 0), "_"), _
                     (New HalconDotNet.HTuple).TupleSelect(hv_CT_Number, 1)), "_"), (New HalconDotNet.HTuple).TupleSelect(hv_CT_Number, _
                    2)), "_"), (New HalconDotNet.HTuple).TupleSelect(hv_CT_Number, 3))
                '配置によって、CTの番号を特定する。
                HalconDotNet.HOperatorSet.TupleFind(hv_CT_IDs, hv_strCT_Number, hv_CT_ID_Index)
                If hv_CT_ID_Index.I = -1 Then
                    ' Stop
                    Continue For
                End If
                hv_CT_ID = (New HalconDotNet.HTuple).TupleAdd(hv_CT_ID_Index, 1)
                If hv_CT_ID.I = 47 Then
                    ' Continue For
                End If
                hv_CTRow = (New HalconDotNet.HTuple).TupleSelect(hv_InRow, hv_CT_NumberIndex)
                hv_CTCol = (New HalconDotNet.HTuple).TupleSelect(hv_InColumn, hv_CT_NumberIndex)

                HalconDotNet.HOperatorSet.ConcatObj(objTargetRegion, ho_InTarget, objTargetRegion) 'SUURI ADD 20150404

                Dim NewCT As New CodedTarget
                NewCT.CT_ID = CInt(hv_CT_ID)
                NewCT.ImageID1 = ImageID
                NewCT.CT_Points.Row = hv_CTRow
                NewCT.CT_Points.Col = hv_CTCol
                NewCT.AllST_Area = (New HalconDotNet.HTuple).TupleSum(hv_AreaST) 'SUURI ADD 20150217
                NewCT.SetlstCTtoST()
                Dim BadCT As Boolean = False
                For Each CT As CodedTarget In lstCT
                    If CT.CT_ID = NewCT.CT_ID Then
                        BadCT = True
                        Exit For

                    End If
                Next
                If BadCT = False Then
                    lstCT.Add(NewCT)
                    All2D.Row = (New HalconDotNet.HTuple).TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                    All2D.Col = (New HalconDotNet.HTuple).TupleConcat(All2D.Col, NewCT.CT_Points.Col)

                End If
                HalconDotNet.HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)
                SP_O = SP_O + 1
                Marshal.ReleaseComObject(ho_CT_Regions)
                HalconDotNet.HOperatorSet.ConcatObj(OTemp(SP_O - 1), ho_ObjectSelected, ho_CT_Regions)
                Marshal.ReleaseComObject(OTemp(SP_O - 1))
                SP_O = 0

            End If

        Next
        HalconDotNet.HOperatorSet.CountObj(ho_CT_Regions, hv_ST_num)
        HalconDotNet.HOperatorSet.CopyObj(ho_CT_Regions, OTemp(SP_O), 1, -1)

        SP_O = SP_O + 1
        Marshal.ReleaseComObject(ho_CT_Regions)
        HalconDotNet.HOperatorSet.FillUp(OTemp(SP_O - 1), ho_CT_Regions)
        Marshal.ReleaseComObject(OTemp(SP_O - 1))
        SP_O = 0

        'Marshal.ReleaseComObject(ho_Region)
        'HalconDotNet.HOperatorSet.Threshold(ho_Image, ho_Region, T_Threshold, 255)
        'Marshal.ReleaseComObject(ho_ConnectedRegions)
        'HalconDotNet.HOperatorSet.Connection(ho_Region, ho_ConnectedRegions)

        Marshal.ReleaseComObject(ho_Difference)
        HalconDotNet.HOperatorSet.Difference(ho_ConnectedRegions, ho_CT_Regions, ho_Difference)
        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Dim param1 As HalconDotNet.HTuple = Nothing
        Dim parammin As HalconDotNet.HTuple = Nothing
        Dim parammax As HalconDotNet.HTuple = Nothing
        param1 = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat("holes_num", "circularity"), "area")
        parammin = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(0, 0.3), 10.0)
        parammax = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(0, 1.0), 999999.0)

        HalconDotNet.HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, "holes_num", "and", 0, 0)
        HalconDotNet.HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "circularity", "and", 0.3, 1.0)
        HalconDotNet.HOperatorSet.SelectShape(ho_SingleTargetRegion, ho_SingleTargetRegion, "area", "and", 10, 999999.0)

        '   HalconDotNet.HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, param1, "and", parammin, parammax)

        'HalconDotNet.HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '               (New HalconDotNet.HTuple).TupleConcat( (New HalconDotNet.HTuple).TupleConcat( _
        '               "holes_num", "circularity"), "area"), _
        '               "and", _
        '                (New HalconDotNet.HTuple).TupleConcat( (New HalconDotNet.HTuple).TupleConcat( _
        '               0, 0.3), 10), _
        '                (New HalconDotNet.HTuple).TupleConcat( (New HalconDotNet.HTuple).TupleConcat( _
        '               0, 1.0), 999999))
        'HalconDotNet.HOperatorSet.SelectShape(ho_Difference, ho_SingleTargetRegion, _
        '              (New HalconDotNet.HTuple).TupleConcat("holes_num", "area"), _
        '              "and",  (New HalconDotNet.HTuple).TupleConcat(0, 10),  (New HalconDotNet.HTuple).TupleConcat(0, 999999))

        'VBMコードターゲットを除外する
#If 1 Then
        Dim ho_RegionDilation As HalconDotNet.HObject = Nothing
        Dim ho_RegionUnion As HalconDotNet.HObject = Nothing
        Dim ho_ConnectedRegions5 As HalconDotNet.HObject = Nothing
        Dim ho_RegionIntersection2 As HalconDotNet.HObject = Nothing
        Dim ho_NonVBMCTRegion As HalconDotNet.HObject = Nothing
        Dim ho_objFourTenTarget As HalconDotNet.HObject = Nothing, ho_objConnectionFourTenTarget As HalconDotNet.HObject = Nothing
        Dim ho_SelectedRegions1 As HalconDotNet.HObject = Nothing, ho_SelectedRegions3 As HalconDotNet.HObject = Nothing

        Dim hv_FourTen_num As HalconDotNet.HTuple = Nothing, hv_Index1 As HalconDotNet.HTuple = Nothing
        Dim hv_Number1 As HalconDotNet.HTuple = Nothing


        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionDilation)
        Marshal.ReleaseComObject(ho_RegionDilation)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionUnion)
        Marshal.ReleaseComObject(ho_RegionUnion)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_RegionIntersection2)
        Marshal.ReleaseComObject(ho_RegionIntersection2)
        HalconDotNet.HOperatorSet.GenEmptyObj(ho_NonVBMCTRegion)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)

        HalconDotNet.HOperatorSet.DilationSeq(ho_SingleTargetRegion, ho_RegionDilation, "c", 3)
        HalconDotNet.HOperatorSet.Union1(ho_RegionDilation, ho_RegionUnion)
        HalconDotNet.HOperatorSet.Connection(ho_RegionUnion, ho_ConnectedRegions5)
        HalconDotNet.HOperatorSet.Intersection(ho_ConnectedRegions5, ho_SingleTargetRegion, ho_RegionIntersection2)
        HalconDotNet.HOperatorSet.SelectShape(ho_RegionIntersection2, ho_NonVBMCTRegion, "connect_num", "and", 4, 4)

        HalconDotNet.HOperatorSet.CountObj(ho_NonVBMCTRegion, hv_FourTen_num)

        For hv_Index1 = 1 To hv_FourTen_num Step 1

            Call HalconDotNet.HOperatorSet.SelectObj(ho_NonVBMCTRegion, ho_objFourTenTarget, hv_Index1)
            Call HalconDotNet.HOperatorSet.Connection(ho_objFourTenTarget, ho_objConnectionFourTenTarget)

            Call HalconDotNet.HOperatorSet.SelectShape(ho_objConnectionFourTenTarget, ho_SelectedRegions1, "area", _
                "and", 10, 99999)

            Call HalconDotNet.HOperatorSet.SelectShape(ho_SelectedRegions1, ho_SelectedRegions3, "circularity", _
                "and", 0.7, 1)

            Call HalconDotNet.HOperatorSet.CountObj(ho_SelectedRegions3, hv_Number1)

            If (New HalconDotNet.HTuple).TupleEqual(hv_Number1, 4) Then

                Get2DCoord(ho_ImageChannel1, ho_SelectedRegions3, hv_CTRow, hv_CTCol, hv_AreaST)

                Dim hvAreaSort As HalconDotNet.HTuple = Nothing
                hvAreaSort = (New HalconDotNet.HTuple).TupleSortIndex(hv_AreaST)
                Dim hvArea012 As HalconDotNet.HTuple = Nothing
                hvArea012 = (hv_AreaST.DArr(hvAreaSort(1)) + hv_AreaST.DArr(hvAreaSort(2)) + hv_AreaST.DArr(hvAreaSort(3))) / 3
                Dim hvArea3 As HalconDotNet.HTuple = Nothing
                hvArea3 = hv_AreaST.DArr(hvAreaSort(0))
                If hvArea012 * 0.75 > hvArea3 Then


                    Dim R0 As HalconDotNet.HTuple = Nothing
                    Dim C0 As HalconDotNet.HTuple = Nothing
                    Dim R1 As HalconDotNet.HTuple = Nothing
                    Dim C1 As HalconDotNet.HTuple = Nothing
                    Dim R2 As HalconDotNet.HTuple = Nothing
                    Dim C2 As HalconDotNet.HTuple = Nothing
                    Dim R3 As HalconDotNet.HTuple = Nothing
                    Dim C3 As HalconDotNet.HTuple = Nothing
                    FourTenTarget1234(hv_CTRow, hv_CTCol, hvAreaSort(0), R0, C0, R1, C1, R2, C2, R3, C3)
                    hv_CTRow = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(R0, R1), R2), R3)
                    hv_CTCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(C0, C1), C2), C3)

                    hv_OutRow = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(R0, R1), R2)
                    hv_OutCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(C0, C1), C2)

                    '大3点で射影変換
                    hv_SRow = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(0, 0), 400)
                    hv_SCol = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(0, 400), 0)
                    HalconDotNet.HOperatorSet.VectorToHomMat2d(hv_OutRow, hv_OutCol, hv_SRow, hv_SCol, hv_HomMat2D)

                    HalconDotNet.HOperatorSet.AffineTransPoint2d(hv_HomMat2D, R3, C3, hv_RowTrans, hv_ColTrans)
                    'R3 = hv_RowTrans(3) - 280
                    'C3 = hv_ColTrans(3) - 280

                    CTemp(SP_C) = hv_RowTrans
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleSub(CTemp(SP_C - 1), 280, hv_RowTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColTrans
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleSub(CTemp(SP_C - 1), 280, hv_ColTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_RowTrans
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleDiv(CTemp(SP_C - 1), 120, hv_RowTrans)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColTrans
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleDiv(CTemp(SP_C - 1), 120, hv_ColTrans)
                    SP_C = 0
                    HalconDotNet.HOperatorSet.TupleRound(hv_RowTrans, hv_RowIndex)
                    HalconDotNet.HOperatorSet.TupleRound(hv_ColTrans, hv_ColIndex)
                    CTemp(SP_C) = hv_RowIndex
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_RowIndex)
                    SP_C = 0
                    CTemp(SP_C) = hv_ColIndex
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleAdd(CTemp(SP_C - 1), 1, hv_ColIndex)
                    SP_C = 0
                    If hv_RowIndex.I = 0 Or hv_ColIndex.I = 0 Then
                        If hv_RowIndex.I = 0 Then
                            hv_CT_Number = 17
                        End If
                        If hv_ColIndex.I = 0 Then
                            hv_CT_Number = 18
                        End If
                    Else
                        hv_CT_Number = (New HalconDotNet.HTuple).TupleAdd((New HalconDotNet.HTuple).TupleMult(4, (New HalconDotNet.HTuple).TupleSub(hv_RowIndex, _
                      1)), hv_ColIndex)
                    End If

                    HalconDotNet.HOperatorSet.TupleSortIndex(hv_CT_Number, hv_CT_NumberIndex)

                    CTemp(SP_C) = hv_CT_Number
                    SP_C = SP_C + 1
                    HalconDotNet.HOperatorSet.TupleSort(CTemp(SP_C - 1), hv_CT_Number)
                    SP_C = 0
                    hv_strCT_Number = hv_CT_Number
                    If hv_CT_Number < 19 And hv_CT_Number > 0 Then
                        hv_strCT_Number = hv_CT_Number

                        Dim NewCT As New CodedTarget
                        NewCT.CT_ID = CInt(hv_CT_Number) + 400
                        NewCT.ImageID1 = ImageID
                        NewCT.CT_Points.Row = hv_CTRow
                        NewCT.CT_Points.Col = hv_CTCol
                        NewCT.AllST_Area = (New HalconDotNet.HTuple).TupleSum(hv_AreaST) 'SUURI ADD 20150217
                        NewCT.SetlstCTtoST()
                        lstCT.Add(NewCT)
                        All2D.Row = (New HalconDotNet.HTuple).TupleConcat(All2D.Row, NewCT.CT_Points.Row)
                        All2D.Col = (New HalconDotNet.HTuple).TupleConcat(All2D.Col, NewCT.CT_Points.Col)
                    End If
                End If
            End If

        Next

        Marshal.ReleaseComObject(ho_ConnectedRegions5)
        Marshal.ReleaseComObject(ho_NonVBMCTRegion)
#End If
        '   HalconDotNet.HOperatorSet.CountObj(ho_SingleTargetRegion, hv_ST_num)
        '  HalconDotNet.HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        ' Marshal.ReleaseComObject(ho_tempObj)
        '  HalconDotNet.HOperatorSet.DilationCircle(ho_SingleTargetRegion, ho_tempObj, 1.5)
        ' HalconDotNet.HOperatorSet.AreaCenterGray(ho_tempObj, ho_ImageChannel1, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        Get2DCoord(ho_ImageChannel1, ho_SingleTargetRegion, hv_ST_Rows, hv_ST_Cols, hv_AreaST)

        HalconDotNet.HOperatorSet.ConcatObj(objTargetRegion, ho_SingleTargetRegion, objTargetRegion) 'SUURI ADD 20150404

        hv_ST_num = (New HalconDotNet.HTuple).TupleLength(hv_ST_Rows)
        '  HalconDotNet.HOperatorSet.AreaCenter(ho_SingleTargetRegion, hv_AreaST, hv_ST_Rows, hv_ST_Cols)
        If lstST Is Nothing Then
            lstST = New List(Of SingleTarget)
        Else
            lstST.Clear()
        End If
        All2D.Row = (New HalconDotNet.HTuple).TupleConcat(All2D.Row, hv_ST_Rows)
        All2D.Col = (New HalconDotNet.HTuple).TupleConcat(All2D.Col, hv_ST_Cols)

        '基準点ターゲットを大白○だけで認識
        Dim intHanteiKyori As Integer = 10

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\flgCTari.txt")
        If IsNumeric(fileContents.Split(" ")(1)) Then
            intHanteiKyori = CInt(fileContents.Split(" ")(1))
        End If
        For Each objtmpTT As CodedTarget In objBeforeTarget.lstCT


            Dim objMinKK As Double = Double.MaxValue
            Dim objMinKK_ST As SingleTarget = Nothing
            hv_ST_num = (New HalconDotNet.HTuple).TupleLength(hv_ST_Rows)
            For hv_ind = 0 To (New HalconDotNet.HTuple).TupleSub(hv_ST_num, 1) Step 1
                HalconDotNet.HOperatorSet.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
                HalconDotNet.HOperatorSet.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
                Dim NewST As New SingleTarget
                NewST.P2D.Row = hv_ST_Row
                NewST.P2D.Col = hv_ST_Col
                NewST.P2ID = hv_ind + 1
                NewST.ImageID = ImageID
                NewST.flgUsed = 0
                HalconDotNet.HOperatorSet.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
                '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217

                Dim tmpKK As HalconDotNet.HTuple = Nothing
                objtmpTT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(hv_ST_Row, hv_ST_Col, tmpKK)
                If objMinKK > tmpKK.ToDArr(0) Then
                    objMinKK = tmpKK
                    objMinKK_ST = NewST
                End If
            Next

            If objMinKK < intHanteiKyori Then
                HalconDotNet.HOperatorSet.TupleRemove(hv_ST_Rows, objMinKK_ST.P2ID - 1, hv_ST_Rows)
                HalconDotNet.HOperatorSet.TupleRemove(hv_ST_Cols, objMinKK_ST.P2ID - 1, hv_ST_Cols)
                Dim NewCT As New CodedTarget
                NewCT.CT_ID = objtmpTT.CT_ID
                NewCT.ImageID1 = objtmpTT.ImageID1
                'Dim oneSTtoCT As New ImagePoints
                'oneSTtoCT.Row =  (New HalconDotNet.HTuple).TupleConcat( (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row),  (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                'oneSTtoCT.Col =  (New HalconDotNet.HTuple).TupleConcat( (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col),  (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))

                NewCT.CT_Points.Row = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row), (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Row, objMinKK_ST.P2D.Row))
                NewCT.CT_Points.Col = (New HalconDotNet.HTuple).TupleConcat((New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col), (New HalconDotNet.HTuple).TupleConcat(objMinKK_ST.P2D.Col, objMinKK_ST.P2D.Col))
                NewCT.AllST_Area = objMinKK_ST.AreaST
                NewCT.SetlstCTtoST()
                lstCT.Add(NewCT)

            End If
        Next
        hv_ST_num = (New HalconDotNet.HTuple).TupleLength(hv_ST_Rows)
        For hv_ind = 0 To (New HalconDotNet.HTuple).TupleSub(hv_ST_num, 1) Step 1
            HalconDotNet.HOperatorSet.TupleSelect(hv_ST_Rows, hv_ind, hv_ST_Row)
            HalconDotNet.HOperatorSet.TupleSelect(hv_ST_Cols, hv_ind, hv_ST_Col)
            Dim NewST As New SingleTarget
            NewST.P2D.Row = hv_ST_Row
            NewST.P2D.Col = hv_ST_Col
            NewST.P2ID = hv_ind + 1
            NewST.ImageID = ImageID
            NewST.flgUsed = 0
            HalconDotNet.HOperatorSet.TupleSelect(hv_AreaST, hv_ind, NewST.AreaST)
            '  NewST.AreaST = hv_AreaST(hv_ind) 'SUURI ADD 20150217
            lstST.Add(NewST)
        Next

        'SUURI ADD 20150217
#If True Then
        insertSTtoCTpoint()

        'For Each objCT As CodedTarget In lstCT
        '    Dim mindistST As Double = Double.MaxValue
        '    Dim minST As SingleTarget = Nothing
        '    '対象のＣＴに一番近いＳＴを取得
        '    For Each objST As SingleTarget In lstST
        '        Dim objKyori As HalconDotNet.HTuple= Nothing
        '        'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
        '        'つまり、ある程度大きさのあるＳＴを選択する。
        '        If objST.AreaST > objCT.AllST_Area Then
        '            objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
        '                                                             objST.P2D.Col,
        '                                                             objKyori)
        '            If objKyori < mindistST Then
        '                mindistST = objKyori
        '                minST = objST
        '            End If
        '        End If
        '    Next
        '    '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
        '    'つまりある程度CTに近いＳＴを選択する。
        '    If mindistST < objCT.GetAllPointsKyoriPixel Then

        '        objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
        '        objCT.CT_Points.Row(0) = minST.P2D.Row
        '        objCT.CT_Points.Col(0) = minST.P2D.Col
        '        objCT.CenterPoint.CopyToMe(minST.P2D)

        '    End If
        'Next

#End If
        'SUURI ADD 20150217 
        All2D_ST.Row = hv_ST_Rows
        All2D_ST.Col = hv_ST_Cols

        Marshal.ReleaseComObject(ho_Region)
        Marshal.ReleaseComObject(ho_ConnectedRegions)
        Marshal.ReleaseComObject(ho_SelectedRegions)
        Marshal.ReleaseComObject(ho_RegionFillUp)
        Marshal.ReleaseComObject(ho_RegionIntersection)
        Marshal.ReleaseComObject(ho_SelectedRegions2)
        Marshal.ReleaseComObject(ho_ConnectedRegions1)
        Marshal.ReleaseComObject(ho_ConnectedRegions2)
        Marshal.ReleaseComObject(ho_ObjectSelected)
        Marshal.ReleaseComObject(ho_ConnectedRegions3)
        Marshal.ReleaseComObject(ho_Waku)
        Marshal.ReleaseComObject(ho_FillWaku)
        Marshal.ReleaseComObject(ho_Contour)
        Marshal.ReleaseComObject(ho_RegressContours)
        Marshal.ReleaseComObject(ho_UnionContours)
        Marshal.ReleaseComObject(ho_ContoursSplit)
        Marshal.ReleaseComObject(ho_RegionIntersection1)
        Marshal.ReleaseComObject(ho_RegionDifference)
        Marshal.ReleaseComObject(ho_ConnectedRegions4)
        Marshal.ReleaseComObject(ho_RegionDifference1)
        Marshal.ReleaseComObject(ho_InTarget)
        Marshal.ReleaseComObject(ho_CT_Regions)
        Marshal.ReleaseComObject(ho_SelectedRegionsT)

        Marshal.ReleaseComObject(ho_SingleTargetRegion)
        Marshal.ReleaseComObject(ho_Difference)
        Marshal.ReleaseComObject(ho_tempObj)
        Marshal.ReleaseComObject(ho_ContourT)
    End Sub


#End If

    Private Sub FourTenTarget1234(ByVal hv_Rows As Object,
                                  ByVal hv_Columns As Object,
                                  ByVal hv_tempIndex As Object,
                                  ByRef R0 As Object,
                                  ByRef C0 As Object,
                                  ByRef R1 As Object,
                                  ByRef C1 As Object,
                                  ByRef R2 As Object,
                                  ByRef C2 As Object,
                                  ByRef R3 As Object,
                                  ByRef C3 As Object)

        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0
        Dim Tuple As New HALCONXLib.HTupleX
        Dim Op As New HALCONXLib.HOperatorSetX

        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing

        hv_C0_Row = Tuple.TupleSelect(hv_Rows, hv_tempIndex)
        hv_C0_Col = Tuple.TupleSelect(hv_Columns, hv_tempIndex)
        'その次に、残りの３頂点を特定する。
        hv_C1_Row = 0
        hv_C1_Col = 0
        hv_C2_Row = 0
        hv_C2_Col = 0
        hv_C3_Row = 0
        hv_C3_Col = 0
        CTemp(SP_C) = hv_Rows
        SP_C = SP_C + 1
        Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
        SP_C = 0
        CTemp(SP_C) = hv_Columns
        SP_C = SP_C + 1
        Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
        SP_C = 0
        hv_blnCheck = -1
        hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
        hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
        hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
        hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
        hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
        hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), hv_C0_Col)
        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
        hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
        If Tuple.TupleGreater(hv_Result, 0) = 1 Then
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
            hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
            If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                hv_I1 = 2
                If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 1
                Else
                    hv_I2 = 1
                    hv_I3 = 0
                End If
            Else
                hv_I1 = 1
                If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 2
                Else
                    hv_I2 = 2
                    hv_I3 = 0
                End If
            End If
        Else
            hv_I1 = 0
            If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                hv_I2 = 1
                hv_I3 = 2
            Else
                hv_I2 = 2
                hv_I3 = 1
            End If
        End If
        hv_C1_Row = Tuple.TupleSelect(hv_Rows, hv_I1)
        hv_C1_Col = Tuple.TupleSelect(hv_Columns, hv_I1)
        hv_C2_Row = Tuple.TupleSelect(hv_Rows, hv_I2)
        hv_C2_Col = Tuple.TupleSelect(hv_Columns, hv_I2)
        hv_C3_Row = Tuple.TupleSelect(hv_Rows, hv_I3)
        hv_C3_Col = Tuple.TupleSelect(hv_Columns, hv_I3)

        R0 = hv_C1_Row
        C0 = hv_C1_Col
        R1 = hv_C3_Row
        C1 = hv_C3_Col
        R2 = hv_C2_Row
        C2 = hv_C2_Col
        R3 = hv_C0_Row
        C3 = hv_C0_Col

        'R3 = hv_C3_Row
        'C3 = hv_C3_Col
        'R2 = hv_C2_Row
        'C2 = hv_C2_Col
        'R1 = hv_C1_Row
        'C1 = hv_C1_Col
        'R0 = hv_C0_Row
        'C0 = hv_C0_Col

    End Sub

    Private Function FourTenTarget1234_ver2(ByVal hv_Rows As Object,
                              ByVal hv_Columns As Object,
                              ByVal hv_tempIndex As Object,
                              ByRef R0 As Object,
                              ByRef C0 As Object,
                              ByRef R1 As Object,
                              ByRef C1 As Object,
                              ByRef R2 As Object,
                              ByRef C2 As Object,
                              ByRef R3 As Object,
                              ByRef C3 As Object)

        Dim CTemp(10) As Object
        Dim SP_C As Integer
        SP_C = 0
        Dim Tuple As New HALCONXLib.HTupleX
        Dim Op As New HALCONXLib.HOperatorSetX

        Dim hv_C0_Row As Object = Nothing, hv_C0_Col As Object = Nothing
        Dim hv_C1_Row As Object = Nothing, hv_C1_Col As Object = Nothing
        Dim hv_C2_Row As Object = Nothing, hv_C2_Col As Object = Nothing
        Dim hv_C3_Row As Object = Nothing, hv_C3_Col As Object = Nothing
        Dim hv_blnCheck As Object = Nothing, hv_Vec1_R As Object = Nothing
        Dim hv_Vec1_C As Object = Nothing, hv_Vec2_R As Object = Nothing
        Dim hv_Vec2_C As Object = Nothing, hv_Vec3_R As Object = Nothing
        Dim hv_Vec3_C As Object = Nothing, hv_Gaiseki1 As Object = Nothing
        Dim hv_Gaiseki2 As Object = Nothing, hv_Result As Object = Nothing
        Dim hv_I1 As Object = Nothing, hv_I2 As Object = Nothing
        Dim hv_I3 As Object = Nothing
        Dim hv_Vec1 As Object = Nothing
        Dim hv_Vec2 As Object = Nothing
        Dim hv_Vec3 As Object = Nothing

        hv_C0_Row = Tuple.TupleSelect(hv_Rows, hv_tempIndex)
        hv_C0_Col = Tuple.TupleSelect(hv_Columns, hv_tempIndex)
        'その次に、残りの３頂点を特定する。
        hv_C1_Row = 0
        hv_C1_Col = 0
        hv_C2_Row = 0
        hv_C2_Col = 0
        hv_C3_Row = 0
        hv_C3_Col = 0
        CTemp(SP_C) = hv_Rows
        SP_C = SP_C + 1
        Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Rows)
        SP_C = 0
        CTemp(SP_C) = hv_Columns
        SP_C = SP_C + 1
        Op.TupleRemove(CTemp(SP_C - 1), hv_tempIndex, hv_Columns)
        SP_C = 0
        hv_blnCheck = -1
        hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), Tuple.TupleSelect(hv_Rows, 1))
        hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), Tuple.TupleSelect(hv_Columns, 1))
        hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), Tuple.TupleSelect(hv_Rows, 2))
        hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), Tuple.TupleSelect(hv_Columns, 2))
        hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), Tuple.TupleSelect(hv_Rows, 0))
        hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), Tuple.TupleSelect(hv_Columns, 0))

        hv_Vec1 = Tuple.TupleAbs(Tuple.TupleMult(hv_Vec1_R, hv_Vec1_C))
        hv_Vec2 = Tuple.TupleAbs(Tuple.TupleMult(hv_Vec2_R, hv_Vec2_C))
        hv_Vec3 = Tuple.TupleAbs(Tuple.TupleMult(hv_Vec3_R, hv_Vec3_C))

        Dim Point0 As Integer

        Dim hv_BigestVec As Object

        If Tuple.TupleGreater(hv_Vec1, hv_Vec2) Then
            If Tuple.TupleGreater(hv_Vec1, hv_Vec3) Then
                'hv_BigestVec = hv_Vec1
                Point0 = 2
            Else
                'hv_BigestVec = hv_Vec3
                Point0 = 1
            End If
        Else
            If Tuple.TupleGreater(hv_Vec2, hv_Vec3) Then
                'hv_BigestVec = hv_Vec2
                Point0 = 0
            Else
                'hv_BigestVec = hv_Vec3
                Point0 = 1

            End If
        End If



        hv_Vec1_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 0), hv_C0_Row)
        hv_Vec1_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 0), hv_C0_Col)
        hv_Vec2_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 1), hv_C0_Row)
        hv_Vec2_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 1), hv_C0_Col)
        hv_Vec3_R = Tuple.TupleSub(Tuple.TupleSelect(hv_Rows, 2), hv_C0_Row)
        hv_Vec3_C = Tuple.TupleSub(Tuple.TupleSelect(hv_Columns, 2), hv_C0_Col)



        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki1)
        CalcGaiseki(hv_Vec1_R, hv_Vec1_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
        hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
        If Tuple.TupleGreater(hv_Result, 0) = 1 Then
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
            CalcGaiseki(hv_Vec2_R, hv_Vec2_C, hv_Vec3_R, hv_Vec3_C, hv_Gaiseki2)
            hv_Result = Tuple.TupleMult(hv_Gaiseki1, hv_Gaiseki2)
            If Tuple.TupleGreater(hv_Result, 0) = 1 Then
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec1_R, hv_Vec1_C, hv_Gaiseki1)
                CalcGaiseki(hv_Vec3_R, hv_Vec3_C, hv_Vec2_R, hv_Vec2_C, hv_Gaiseki2)
                hv_I1 = 2
                If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 1
                Else
                    hv_I2 = 1
                    hv_I3 = 0
                End If
            Else
                hv_I1 = 1
                If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                    hv_I2 = 0
                    hv_I3 = 2
                Else
                    hv_I2 = 2
                    hv_I3 = 0
                End If
            End If
        Else
            hv_I1 = 0
            If Tuple.TupleLess(hv_Gaiseki1, 0) = 1 Then
                hv_I2 = 1
                hv_I3 = 2
            Else
                hv_I2 = 2
                hv_I3 = 1
            End If
        End If

        If Not hv_I1 = Point0 Then
            Return -1
        End If


        hv_C1_Row = Tuple.TupleSelect(hv_Rows, hv_I1)
        hv_C1_Col = Tuple.TupleSelect(hv_Columns, hv_I1)
        hv_C2_Row = Tuple.TupleSelect(hv_Rows, hv_I2)
        hv_C2_Col = Tuple.TupleSelect(hv_Columns, hv_I2)
        hv_C3_Row = Tuple.TupleSelect(hv_Rows, hv_I3)
        hv_C3_Col = Tuple.TupleSelect(hv_Columns, hv_I3)

        R0 = hv_C1_Row
        C0 = hv_C1_Col
        R1 = hv_C3_Row
        C1 = hv_C3_Col
        R2 = hv_C2_Row
        C2 = hv_C2_Col
        R3 = hv_C0_Row
        C3 = hv_C0_Col

        Return 0

    End Function




    ''' <summary>
    ''' 未使用
    ''' </summary>
    ''' 3チャンネル画像に対してBinThresholdを掛け3つの領域を結合する。
    Private Sub GetFirstRegion(ByRef ho_Image As HALCONXLib.HUntypedObjectX, ByRef ho_Region As HALCONXLib.HUntypedObjectX)
        Dim ho_Image1 As HUntypedObjectX = Nothing
        Dim ho_Image2 As HUntypedObjectX = Nothing
        Dim ho_Image3 As HUntypedObjectX = Nothing
        Dim ho_Region1 As HUntypedObjectX = Nothing, ho_RegionComplement1 As HUntypedObjectX = Nothing
        Dim ho_Region2 As HUntypedObjectX = Nothing, ho_RegionComplement2 As HUntypedObjectX = Nothing
        Dim ho_Region3 As HUntypedObjectX = Nothing, ho_RegionComplement3 As HUntypedObjectX = Nothing
        Dim ho_RegionUnion As HUntypedObjectX = Nothing

        ' Initialize local and output iconic variables 
        Op.GenEmptyObj(ho_Image1)
        Op.GenEmptyObj(ho_Image2)
        Op.GenEmptyObj(ho_Image3)
        Op.GenEmptyObj(ho_Region1)
        Op.GenEmptyObj(ho_RegionComplement1)
        Op.GenEmptyObj(ho_Region2)
        Op.GenEmptyObj(ho_RegionComplement2)
        Op.GenEmptyObj(ho_Region3)
        Op.GenEmptyObj(ho_RegionComplement3)
        Op.GenEmptyObj(ho_RegionUnion)

        Marshal.ReleaseComObject(ho_Image1)
        Marshal.ReleaseComObject(ho_Image2)
        Marshal.ReleaseComObject(ho_Image3)
        Op.Decompose3(ho_Image, ho_Image1, ho_Image2, ho_Image3)
        Marshal.ReleaseComObject(ho_Region1)
        Op.BinThreshold(ho_Image1, ho_Region1)
        Marshal.ReleaseComObject(ho_RegionComplement1)
        Op.Complement(ho_Region1, ho_RegionComplement1)
        Marshal.ReleaseComObject(ho_Region2)
        Op.BinThreshold(ho_Image2, ho_Region2)
        Marshal.ReleaseComObject(ho_RegionComplement2)
        Op.Complement(ho_Region2, ho_RegionComplement2)
        Marshal.ReleaseComObject(ho_Region3)
        Op.BinThreshold(ho_Image3, ho_Region3)
        Marshal.ReleaseComObject(ho_RegionComplement3)
        Op.Complement(ho_Region3, ho_RegionComplement3)
        Marshal.ReleaseComObject(ho_RegionUnion)
        Op.Union2(ho_RegionComplement1, ho_RegionComplement2, ho_RegionUnion)
        Marshal.ReleaseComObject(ho_Region)
        Op.Union2(ho_RegionUnion, ho_RegionComplement3, ho_Region)

        Marshal.ReleaseComObject(ho_Image1)
        Marshal.ReleaseComObject(ho_Image2)
        Marshal.ReleaseComObject(ho_Image3)
        Marshal.ReleaseComObject(ho_Region1)
        Marshal.ReleaseComObject(ho_RegionComplement1)
        Marshal.ReleaseComObject(ho_Region2)
        Marshal.ReleaseComObject(ho_RegionComplement2)
        Marshal.ReleaseComObject(ho_Region3)
        Marshal.ReleaseComObject(ho_RegionComplement3)
        Marshal.ReleaseComObject(ho_RegionUnion)

    End Sub

    ''' <summary>
    ''' '3チャンネル画像のGreen画像のみに対してBinThresholdを掛けて領域を抽出する。
    ''' </summary>
    Private Sub GetFirstRegionG(ByRef ho_Image As HALCONXLib.HUntypedObjectX, _
                                ByRef ho_Region As HALCONXLib.HUntypedObjectX, _
                                ByRef ImageChannel1 As HALCONXLib.HUntypedObjectX, _
                                ByVal intThreshold As Integer)
        Dim Op As New HALCONXLib.HOperatorSetX

        ' Dim ho_Image2 As HUntypedObjectX = Nothing
        Dim ho_Region2 As HUntypedObjectX = Nothing
        Dim ho_EmphasizeImage As HUntypedObjectX = Nothing

        Op.GenEmptyObj(ImageChannel1)
        Op.GenEmptyObj(ho_Region2)
        Op.GenEmptyObj(ho_EmphasizeImage)
        Dim HH As New Object
        Dim WW As New Object
        Op.GetImagePointer1(ho_Image, Nothing, Nothing, WW, HH)
        Debug.Print(WW & "   " & HH)
        Dim channelcount As New Object
        Op.CountChannels(ho_Image, channelcount)
        Marshal.ReleaseComObject(ImageChannel1)
        If channelcount = 1 Then
            Op.CopyImage(ho_Image, ImageChannel1)
        ElseIf channelcount = 3 Then
            Op.Decompose3(ho_Image, Nothing, ImageChannel1, Nothing)
        End If


        Marshal.ReleaseComObject(ho_EmphasizeImage)
        Op.Emphasize(ImageChannel1, ho_EmphasizeImage, 7, 7, 1)

        Marshal.ReleaseComObject(ho_Region2)
        'Op.BinThreshold(ho_EmphasizeImage, ho_Region2)
        Op.BinThreshold(ImageChannel1, ho_Region2)
        Op.Complement(ho_Region2, ho_Region)
        If intThreshold > 1 Then
            Op.Threshold(ho_Image, ho_Region, intThreshold, 255)
            'Op.Threshold(ho_EmphasizeImage, ho_Region, intThreshold, 255)
        End If

        ' Marshal.ReleaseComObject(ho_Image2)
        Marshal.ReleaseComObject(ho_Region2)
        Marshal.ReleaseComObject(ho_EmphasizeImage)
    End Sub
    Private Sub Get2DCoord(ByRef ho_Image As HALCONXLib.HUntypedObjectX, ByRef InTarget As HALCONXLib.HUntypedObjectX, ByRef Row As Object, ByRef Col As Object, ByRef hvArea As Object)
        Dim ho_DilationCircle As HALCONXLib.HUntypedObjectX = Nothing
        Op.GenEmptyObj(ho_DilationCircle)
        Marshal.ReleaseComObject(ho_DilationCircle)
        Op.DilationCircle(InTarget, ho_DilationCircle, 1.5)
#If True Then

        Try
            'Dim ho_RegionUnion As HALCONXLib.HUntypedObjectX = Nothing
            'Dim ho_ReduceImage As HALCONXLib.HUntypedObjectX = Nothing
            'Dim ho_Edges As HALCONXLib.HUntypedObjectX = Nothing
            'Op.GenEmptyObj(ho_RegionUnion)
            'Op.GenEmptyObj(ho_ReduceImage)
            'Op.GenEmptyObj(ho_Edges)
            'Marshal.ReleaseComObject(ho_RegionUnion)
            'Op.Union1(ho_DilationCircle, ho_RegionUnion)
            'Marshal.ReleaseComObject(ho_ReduceImage)
            'Op.ReduceDomain(ho_Image, ho_RegionUnion, ho_ReduceImage)
            '' Op.AutoThreshold (ho_Dilation
            'Marshal.ReleaseComObject(ho_Edges)
            'Op.EdgesColorSubPix(ho_ReduceImage, ho_Edges, "canny", 1, 20, 40)
            'Try
            '    Op.FitEllipseContourXld(ho_Edges, "fitzgibbon", -1, 0, 0, 200, 3, 2, Row, Col, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            'Catch ex As Exception
            'End Try
            ''  Op.EdgesSubPix(ho_ReduceImage, ho_Edges, "canny", 1, 20, 40)
            'Op.AreaCenterXld(ho_Edges, hvArea, Row, Col, Nothing)
            'Op.AreaCenterPointsXld(ho_Edges, hvArea, Row, Col)
            'Marshal.ReleaseComObject(ho_RegionUnion)
            'Marshal.ReleaseComObject(ho_ReduceImage)
            'Marshal.ReleaseComObject(ho_Edges)

            'Op.AreaCenterGray(ho_DilationCircle, ho_Image, hvArea, Row, Col)
            Op.AreaCenter(InTarget, hvArea, Row, Col)
            Marshal.ReleaseComObject(ho_DilationCircle)

        Catch ex As Exception
            'Op.AreaCenterGray(ho_DilationCircle, ho_Image, hvArea, Row, Col)
            Op.AreaCenter(InTarget, hvArea, Row, Col)
            Marshal.ReleaseComObject(ho_DilationCircle)

        End Try

        ' Dim Op As New HALCONXLib.HOperatorSetX
#End If



        ' Op.AreaCenterGray(InTarget, ho_Image, Nothing, Row, Col)

    End Sub

    Public Function GetST_MaxID() As Integer
        GetST_MaxID = 0
        For Each ST As SingleTarget In lstST
            If GetST_MaxID < ST.P2ID Then
                GetST_MaxID = ST.P2ID
            End If
        Next
    End Function

    Public Sub insertSTtoCTpoint()
        For Each objCT As CodedTarget In lstCT
            If objCT.CT_ID > 400 And objCT.CT_ID < 419 Then
                Continue For
            End If
            Dim mindistST As Double = Double.MaxValue
            Dim minST As SingleTarget = Nothing
            '対象のＣＴに一番近いＳＴを取得
            For Each objST As SingleTarget In lstST
                Dim objKyori As Object = Nothing
                'ただし、そのＳＴの面積がＣＴの内部点の全面積より大きい場合
                'つまり、ある程度大きさのあるＳＴを選択する。
                If objST.AreaST > objCT.AllST_Area Then
                    objCT.lstCTtoST.Item(0).P2D.CalcDistToInputPoint(objST.P2D.Row,
                                                                     objST.P2D.Col,
                                                                     objKyori)
                    If objKyori < mindistST Then
                        mindistST = objKyori
                        minST = objST
                    End If
                End If
            Next
            '一番近いＳＴがＣＴの内部点間の距離より小さいければＣＴの一番目の点（代表点）に置き換える
            'つまりある程度CTに近いＳＴを選択する。
            If mindistST < objCT.GetAllPointsKyoriPixel * 2 Then

                objCT.lstCTtoST.Item(0).P2D.CopyToMe(minST.P2D)
                objCT.CT_Points.Row(0) = minST.P2D.Row
                objCT.CT_Points.Col(0) = minST.P2D.Col
                objCT.CenterPoint.CopyToMe(minST.P2D)

            End If
        Next
    End Sub

    Public Sub OneSTtoCTpoint(ByVal objST As SingleTarget, ByVal strCTpoint As String)
        For Each objCT As CodedTarget In lstCT
            If objCT.CT_ID = CInt(strCTpoint) Then
                objCT.lstCTtoST.Item(0).P2D.CopyToMe(objST.P2D)
                objCT.CT_Points.Row(0) = objST.P2D.Row
                objCT.CT_Points.Col(0) = objST.P2D.Col
                objCT.CenterPoint.CopyToMe(objST.P2D)

                objCT.DeleteDataS()
                objCT.SaveData()

            End If
        Next
    End Sub




    Private Sub makeCombination(comb As List(Of String), ByVal n As Integer, ByVal r As Integer, ByVal Steper As Integer, ByVal init As Integer, ByVal CombList As List(Of Integer()), ByRef CmbCount As Integer)

        Try
            'r桁すべてが生成された
            If Steper = r Then
                Dim patern(3) As Integer
                For j As Integer = 0 To r - 1
                    'Debug.Print(comb(j))
                    patern(j) = comb(j)
                Next
                CombList.Add(patern)

                'Debug.Print(vbCrLf)
                CmbCount = CmbCount + 1
                Exit Sub

            End If

            ' 全桁生成されていない
            For i As Integer = init To n - r + Steper
                comb.Add(i)

                makeCombination(comb, n, r, Steper + 1, i + 1, CombList, CmbCount)
                ' ここで配列から一つ排除しないと困ったことになる
                comb.Remove(i)
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub




End Class
