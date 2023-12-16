Imports System.Data.Odbc
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.ComponentModel.Design

Public Class transaksi
    Dim connectionString As String = "Driver={Mysql ODBC 3.51 Driver};Database=booking_lapangan;Server=localhost;uid=root"
    Public selectedValue As String
    Public stok As Integer
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Dim longpaper As Integer
    Dim conn As OdbcConnection
    Dim Da As OdbcDataAdapter
    Dim Ds As DataSet
    Dim Cmd As OdbcCommand
    Dim Rd As OdbcDataReader
    Dim MyDB As String
    Sub koneksi()
        MyDB = "Driver={Mysql ODBC 3.51 Driver};Database=booking_lapangan;Server=localhost;uid=root"
        conn = New OdbcConnection(MyDB)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
    End Sub
    Sub kondisiAwal()
        id.Text = ""
        nama.Text = ""
        harga.Text = ""
        total.Text = ""
        Call koneksi()
        Da = New OdbcDataAdapter("Select * from transaksi", conn)
        Ds = New DataSet
        Da.Fill(Ds, "transaksi")
        DataGridView1.DataSource = Ds.Tables("transaksi")
    End Sub
    Sub cmbdata()
        Using conn As New OdbcConnection(connectionString)
            conn.Open()
            Using cmd As New OdbcCommand("select * from member", conn)
                Dim sdr As OdbcDataReader = cmd.ExecuteReader
                id.Items.Clear()
                While sdr.Read()
                    id.Items.Add(sdr("id"))
                End While
                sdr.Close()
            End Using
            conn.Close()
        End Using
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            selectedValue = durasi.SelectedItem
            Dim jumlah = selectedValue.Split(" ")(0)
            If id.Text = "" Or nama.Text = "" Or durasi.Text = "" Or harga.Text = "" Or total.Text = "" Then
                MsgBox("Data harus lengkap", vbExclamation, "Pesan")
            Else

                Dim simpan As String = "insert into transaksi (id_booking,nama,durasi,harga_lapangan,total_harga) values ('" & id.Text & "', '" & nama.Text & "', '" & durasi.Text & "', '" & harga.Text * jumlah & "', '" & total.Text & "')"

                Cmd = New OdbcCommand(simpan, conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Data berhasil di simpan", vbInformation, "Simpan")
                Call kondisiAwal()
            End If
        Catch ex As Exception
            MsgBox("Terdapat kesalahan" & ex.Message)
        End Try
    End Sub

    Private Sub id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles id.SelectedIndexChanged

        Call koneksi()
        Cmd = New OdbcCommand("select * from member where id = '" & id.Text & "'", conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        harga.Text = Rd.Item(6)
    End Sub

    Private Sub transaksi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        kondisiAwal()
        cmbdata()
    End Sub
    Sub changelongpaper()
        Dim rowcount As Integer
        longpaper = 0
        rowcount = DataGridView1.Rows.Count
        longpaper = rowcount * 15
        longpaper = longpaper + 240
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        changelongpaper()
        PPD.Document = PD
        PPD.ShowDialog()
    End Sub

    Private Sub PD_BeginPrint(sender As Object, e As PrintEventArgs) Handles PD.BeginPrint
        'Dim pagesetup As New PageSettings
        'pagesetup.PaperSize = New PaperSize("Custom", 300, 500) 'fixed size
        Dim paperSize As New PaperSize("Custom", 300, 500)
        'pagesetup.PaperSize = New PaperSize("Custom", 250, longpaper)
        PD.DefaultPageSettings.PaperSize = paperSize
    End Sub

    Private Sub PD_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PD.PrintPage
        Dim f10 As New Font("Times New Roman", 10, FontStyle.Regular)
        Dim f10b As New Font("Times New Roman", 10, FontStyle.Bold)
        Dim f14 As New Font("Times New Roman", 14, FontStyle.Bold)

        Dim leftmargin As Integer = PD.DefaultPageSettings.Margins.Left
        Dim centermargin As Integer = PD.DefaultPageSettings.PaperSize.Width / 2
        Dim rightmargin As Integer = PD.DefaultPageSettings.PaperSize.Width

        Dim kanan As New StringFormat
        Dim tengah As New StringFormat
        kanan.Alignment = StringAlignment.Far
        tengah.Alignment = StringAlignment.Center

        Dim garis As String
        garis = "------------------------------------------------------------------"

        e.Graphics.DrawString("Lapangan", f14, Brushes.Black, centermargin, 5, tengah)
        e.Graphics.DrawString("JL.Raya Mangun Jaya, Tambun Selatan, Bekasi," & vbNewLine & " Kab Bekasi, Jawa Barat, 17510", f10, Brushes.Black, centermargin, 30, tengah)
        e.Graphics.DrawString("Hp: 0858-1743-3974", f10, Brushes.Black, centermargin, 65, tengah)

        e.Graphics.DrawString("Nama Kasir :", f10, Brushes.Black, 0, 90)
        e.Graphics.DrawString("Titan", f10, Brushes.Black, 75, 90)

        e.Graphics.DrawString(Date.Now(), f10, Brushes.Black, 0, 105)
        e.Graphics.DrawString("Nama", f10, Brushes.Black, 0, 125)
        e.Graphics.DrawString("durasi", f10, Brushes.Black, 80, 125)
        e.Graphics.DrawString("harga", f10, Brushes.Black, 130, 125)
        e.Graphics.DrawString("Total", f10, Brushes.Black, rightmargin, 125, kanan)
        e.Graphics.DrawString(garis, f10, Brushes.Black, 0, 130)
        Dim tinggi As Integer
        Dim total As Integer
        For Each baris As DataGridViewRow In DataGridView1.Rows
            If Not baris.IsNewRow Then
                tinggi += 15
                e.Graphics.DrawString(baris.Cells(2).Value, f10, Brushes.Black, 0, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(3).Value, f10, Brushes.Black, 80, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(4).Value, f10, Brushes.Black, 130, 130 + tinggi)

                e.Graphics.DrawString(baris.Cells(5).Value, f10, Brushes.Black, rightmargin, 130 + tinggi, kanan)
                total += CDbl(baris.Cells(5).Value)
            End If
        Next
        tinggi = 140 + tinggi
        e.Graphics.DrawString(garis, f10, Brushes.Black, 0, tinggi)
        e.Graphics.DrawString("Subtotal :" & FormatCurrency(total), f10b, Brushes.Black, 150, 15 + tinggi)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
        Me.Hide()

    End Sub



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Call koneksi()
            Dim hapusdata As String = "Delete from transaksi"
            Cmd = New OdbcCommand(hapusdata, conn)
            Cmd.ExecuteReader()
            Call kondisiAwal()
        Catch ex As Exception
            MsgBox("Terdapat kesalahan" & ex.Message)
        End Try
    End Sub

    Private Sub durasi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles durasi.SelectedIndexChanged
        selectedValue = durasi.SelectedItem
        Dim jumlah = selectedValue.Split(" ")(0)
        total.Text = CDbl(jumlah) * CDbl(harga.Text)
    End Sub
End Class