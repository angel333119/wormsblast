using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace worm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo DAT ou AUD|*.dat; *.aud|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo .DAT ou .AUD do jogo Worms Blast...";
            openFileDialog1.Multiselect = true;

            int tamanho;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);

                        long magicdat = br.ReadInt64();

                        if (magicdat == 0) //Arquivo Texto
                        {
                            int quantidadedetextos = br.ReadInt32();

                            br.BaseStream.Seek(0x1C, SeekOrigin.Begin);

                            string arquivotxt = "";

                            for (int i = 0; i < quantidadedetextos; i++)
                            {
                                int id = br.ReadInt32();

                                int quantidadeletras = br.ReadInt32();

                                byte[] bytestexto = br.ReadBytes(quantidadeletras);

                                string convertido = Encoding.UTF8.GetString(bytestexto);

                                arquivotxt += convertido + "\r\n";
                            }
                            File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + ".txt", arquivotxt);
                        }
                        else
                        {
                            br.BaseStream.Seek(0x48, SeekOrigin.Begin);

                            int quantidadedewav = br.ReadInt32();

                            br.BaseStream.Seek(0x114, SeekOrigin.Begin);

                            int pulo = br.ReadInt32();

                            br.BaseStream.Seek(0x118, SeekOrigin.Begin);

                            int pulo2 = br.ReadInt32();

                            br.BaseStream.Seek(pulo * 4 + pulo2 + 4, SeekOrigin.Current);

                            int verificador = br.ReadInt32();

                            int tamanhoarquivoatual = 0;

                            br.BaseStream.Seek(-8, SeekOrigin.Current);

                            if (verificador == 0x70474156) // Arquivo de PS2 - VAG
                            {
                                for (int i = 1; i <= quantidadedewav + 1; i++)
                                {
                                    if (i <= quantidadedewav)
                                    {
                                        tamanhoarquivoatual = br.ReadInt32();

                                        tamanho = tamanhoarquivoatual;

                                        byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                        string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                        Directory.CreateDirectory(caminhodapasta);

                                        string arquivowav = Path.Combine(caminhodapasta, i + ".vag");

                                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                        {
                                            bw.Write(arquivo);
                                        }
                                    }
                                    else
                                    {
                                        tamanhoarquivoatual = br.ReadInt32();

                                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                                        tamanho = tamanhoarquivoatual;

                                        byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                        string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                        Directory.CreateDirectory(caminhodapasta);

                                        string arquivowav = Path.Combine(caminhodapasta, i + ".bin");

                                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                        {
                                            bw.Write(arquivo);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 1; i <= quantidadedewav + 1; i++)
                                {
                                    if (i <= quantidadedewav)
                                    {
                                        tamanhoarquivoatual = br.ReadInt32();

                                        tamanho = tamanhoarquivoatual;

                                        byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                        string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                        Directory.CreateDirectory(caminhodapasta);

                                        string arquivowav = Path.Combine(caminhodapasta, i + ".wav");

                                        int magic = 0x46464952;
                                        long WAVEfmt = 0x20746D6645564157;
                                        long parte1 = 0x0001000100000010;
                                        long parte2 = 0x0000AC4400005622;
                                        long parte3 = 0x6174616400100002;

                                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                        {
                                            bw.Write(magic);
                                            bw.Write(tamanho + 0x24);
                                            bw.Write(WAVEfmt);
                                            bw.Write(parte1);
                                            bw.Write(parte2);
                                            bw.Write(parte3);
                                            bw.Write(tamanho);
                                            bw.Write(arquivo);
                                        }
                                    }
                                    else
                                    {
                                        tamanhoarquivoatual = br.ReadInt32();

                                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                                        tamanho = tamanhoarquivoatual;

                                        byte[] arquivo = br.ReadBytes(tamanhoarquivoatual);

                                        string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                                        Directory.CreateDirectory(caminhodapasta);

                                        string arquivowav = Path.Combine(caminhodapasta, i + ".bin");

                                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(arquivowav)))
                                        {
                                            bw.Write(arquivo);
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                }
                MessageBox.Show("Terminado!", "AVISO!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo DAT ou AUD|*.dat; *.aud|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo .DAT ou .AUD do jogo Worms Blast...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);
                        BinaryWriter bw = new BinaryWriter(stream);

                        long magicdat = br.ReadInt64();

                        if (magicdat == 0) //Arquivo DAT
                        {
                            int quantidadedetextos = br.ReadInt32();

                            br.BaseStream.Seek(0x1C, SeekOrigin.Begin);

                            var arquivotxt = File.ReadLines(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + ".txt");

                            int[] id = new int[quantidadedetextos];

                            for (int i = 0; i < quantidadedetextos; i++)
                            {
                                id[i] = br.ReadInt32();

                                int quantidadeletras = br.ReadInt32();

                                br.BaseStream.Seek(quantidadeletras, SeekOrigin.Current);
                            }
                            stream.SetLength(0x1C);

                            int n = 0;

                            foreach(var line in arquivotxt)
                            {
                                bw.Write(id[n]);
                                byte[] bytestexto = Encoding.UTF8.GetBytes(line);
                                int quantidadeletras = bytestexto.Length;
                                bw.Write(quantidadeletras);
                                bw.Write(bytestexto);
                                n++;
                            }
                        }
                        else // AUD
                        {
                            string caminhodapasta = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                            if (Directory.Exists(caminhodapasta))
                            {
                                br.BaseStream.Seek(0x48, SeekOrigin.Begin);

                                int quantidadedewav = br.ReadInt32();

                                br.BaseStream.Seek(0x114, SeekOrigin.Begin);

                                int pulo = br.ReadInt32();

                                br.BaseStream.Seek(0x118, SeekOrigin.Begin);

                                int pulo2 = br.ReadInt32();

                                br.BaseStream.Seek(pulo * 4 + pulo2 + 4, SeekOrigin.Current);

                                int verificador = br.ReadInt32();

                                stream.SetLength(0x800);

                                br.BaseStream.Seek(-8, SeekOrigin.Current);

                                if (verificador == 0x70474156) // Arquivo de audio PS2 - VAG
                                {
                                    for (int i = 1; i <= quantidadedewav + 1; i++)
                                    {
                                        if (i <= quantidadedewav)
                                        {
                                            byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".vag"));
                                            int tamanhonovo = bytesarquivoaudio.Length;
                                            bw.Write(tamanhonovo);
                                            bw.Write(bytesarquivoaudio);
                                        }
                                        else
                                        {
                                            byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".bin"));
                                            bw.Write(bytesarquivoaudio);
                                        }
                                    }
                                }
                                else // Arquivo de audio PC - WAV
                                {
                                    for (int i = 1; i <= quantidadedewav + 1; i++)
                                    {
                                        if (i <= quantidadedewav)
                                        {
                                            byte[] bytesarquivoaudio = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".wav"));

                                            int novoTamanho = bytesarquivoaudio.Length - 0x2C;
                                            byte[] novoArray = new byte[novoTamanho];
                                            Array.Copy(bytesarquivoaudio, 0x2C, novoArray, 0, novoTamanho);

                                            bw.Write(novoTamanho);
                                            bw.Write(novoArray);
                                        }
                                        else
                                        {
                                            byte[] bytesarquivo = File.ReadAllBytes(Path.Combine(caminhodapasta, i + ".bin"));
                                            bw.Write(bytesarquivo);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("A pasta \"" + Path.GetFileNameWithoutExtension(file) + "\" não foi encontrada", "AVISO!");
                                return;
                            }
                        }
                    }
                }
                MessageBox.Show("Terminado!", "AVISO!");
            }
        }
    }
}