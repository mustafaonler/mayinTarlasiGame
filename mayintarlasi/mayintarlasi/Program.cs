using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayintarlasi
{
    class Program
    {
     
        
            
        
            // Oyun Alanı
            static char[,] gameBoard;
            static bool[,] revealed;
            static bool[,] mines;
            static int width = 8; // Genişlik
            static int height = 8; // Yükseklik
            static int mineCount = 10; // Mayın sayısı

            // Ana fonksiyon (Main method)
            static void Main()
            {
                // Oyun alanını başlat
                InitializeGame();

                bool gameOver = false;

                while (!gameOver)
                {
                    PrintBoard();

                    // Kullanıcıdan hücre seçmesi istenir
                    Console.Write("Satır (0-7) girin: ");
                    int row = int.Parse(Console.ReadLine());

                    Console.Write("Sütun (0-7) girin: ");
                    int col = int.Parse(Console.ReadLine());

                    if (IsValidMove(row, col))
                    {
                        if (mines[row, col]) // Eğer mayına basıldıysa
                        {
                            Console.WriteLine("Mayına bastınız! Oyun bitti.");
                            gameOver = true;
                        }
                        else
                        {
                            // Seçilen hücreyi aç
                            RevealCell(row, col);

                            if (IsGameWon()) // Kazanma kontrolü
                            {
                                Console.WriteLine("Tebrikler, oyunu kazandınız!");
                                gameOver = true;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçim. Lütfen geçerli bir satır ve sütun girin.");
                    }
                }
            }

            // Oyun alanını başlatma
            static void InitializeGame()
            {
                gameBoard = new char[height, width];
                revealed = new bool[height, width];
                mines = new bool[height, width];

                // Mayınları yerleştir
                Random rand = new Random();
                for (int i = 0; i < mineCount; i++)
                {
                    int r, c;
                    do
                    {
                        r = rand.Next(height);
                        c = rand.Next(width);
                    } while (mines[r, c]); // Aynı yere mayın yerleştirilmesin

                    mines[r, c] = true;
                }

                // Oyun alanını oluştur
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        if (mines[r, c])
                        {
                            gameBoard[r, c] = '*'; // Mayınları * ile işaretle
                        }
                        else
                        {
                            gameBoard[r, c] = '0'; // Hücreyi '0' olarak başlat
                        }
                    }
                }

                // Sayıları hesapla (her hücre çevresindeki mayın sayısı)
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        if (mines[r, c]) continue;

                        int mineCountAround = CountMinesAround(r, c);
                        gameBoard[r, c] = mineCountAround.ToString()[0]; // O hücrenin etrafındaki mayın sayısını yaz
                    }
                }
            }

            // Etrafındaki mayın sayısını hesapla
            static int CountMinesAround(int row, int col)
            {
                int mineCountAround = 0;

                for (int r = row - 1; r <= row + 1; r++)
                {
                    for (int c = col - 1; c <= col + 1; c++)
                    {
                        if (r >= 0 && r < height && c >= 0 && c < width)
                        {
                            if (mines[r, c])
                            {
                                mineCountAround++;
                            }
                        }
                    }
                }

                return mineCountAround;
            }

            // Geçerli bir hamle olup olmadığını kontrol et
            static bool IsValidMove(int row, int col)
            {
                return row >= 0 && row < height && col >= 0 && col < width && !revealed[row, col];
            }

            // Hücreyi aç
            static void RevealCell(int row, int col)
            {
                if (revealed[row, col]) return;

                revealed[row, col] = true;

                // Eğer etrafında hiç mayın yoksa, etrafındaki hücreleri de aç
                if (gameBoard[row, col] == '0')
                {
                    for (int r = row - 1; r <= row + 1; r++)
                    {
                        for (int c = col - 1; c <= col + 1; c++)
                        {
                            if (r >= 0 && r < height && c >= 0 && c < width)
                            {
                                RevealCell(r, c); // Rekürsif olarak etrafı aç
                            }
                        }
                    }
                }
            }

            // Oyun bitip bitmediğini kontrol et
            static bool IsGameWon()
            {
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        if (!mines[r, c] && !revealed[r, c])
                        {
                            return false; // Eğer açılmamış bir hücre varsa, oyun bitmemiştir
                        }
                    }
                }
                return true; // Tüm güvenli hücreler açıldığında kazanılır
            }

            // Oyun alanını ekrana yazdır
            static void PrintBoard()
            {
                Console.Clear();

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        if (revealed[r, c])
                        {
                            Console.Write(gameBoard[r, c] + " ");
                        }
                        else
                        {
                            Console.Write("X "); // Açılmamış hücreyi 'X' ile göster
                        }
                    }
                    Console.WriteLine();
                }
            Console.ReadLine();
            }
       
        }
    }
    

