Imports System.Data.Odbc

Public Class member
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
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox1.Text = .Cells(0).Value
            Me.TextBox2.Text = .Cells(1).Value
            Me.TextBox3.Text = .Cells(2).Value
            Me.ComboBox1.Text = .Cells(4).Value
            Me.ComboBox2.Text = .Cells(5).Value
        End With
    End Sub

    Private Sub member_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = Today
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        harga.Text = ""
        Button2.Text = "KIRIM"
        Button3.Text = "RISET"
        Button4.Text = "EDIT"
        Button5.Text = "HAPUS"
        Call koneksi()
        Da = New OdbcDataAdapter("Select * from member", conn)
        Ds = New DataSet
        Da.Fill(Ds, "member")
        DataGridView1.DataSource = Ds.Tables("member")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Data harus lengkap", vbExclamation, "Pesan")
            Else
                Cmd = New OdbcCommand("select * from member where id = '" & TextBox1.Text & "'", conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                If Not Rd.HasRows Then
                    Dim simpan As String = "insert into member (id, nama, telepon, mulai, selesai,harga) values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & ComboBox1.Text & "','" & ComboBox2.Text & "', '" & harga.Text & "')"

                    Cmd = New OdbcCommand(simpan, conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Data berhasil di simpan", vbInformation, "Simpan")
                    Call KondisiAwal()
                Else
                    MsgBox("data sudah ada")
                    TextBox1.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox("Terdapat kesalahan" & ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MsgBox("Pastikan semua field terisi")
        Else
            Call koneksi()
            Dim EditData As String = "update member set Id='" & TextBox1.Text & "', Nama= '" & TextBox2.Text & "', telepon= '" & TextBox3.Text & "', harga='" & harga.Text & "' where Id = '" & TextBox1.Text & "' "
            Cmd = New OdbcCommand(EditData, conn)
            Cmd.ExecuteNonQuery()
            MsgBox("Edit Data Berhasil")
            Call KondisiAwal()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If MessageBox.Show("Apakah anda yakin ingin menghapus data ?", "info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim hapusdata As String = "Delete from member where id = '" & TextBox1.Text & "'"
            Cmd = New OdbcCommand(hapusdata, conn)
            Cmd.ExecuteReader()
            Call KondisiAwal()
            MsgBox("Data berhasil dihapus", MsgBoxStyle.Information, "Information")

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub
End Class