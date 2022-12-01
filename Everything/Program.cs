using System;
using System.Linq;
using System.Xml.Schema;

namespace Everything
{
    class Program
    {
        static char[] letters = {
                'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и',
                'і', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю',
                'я'
            };
        static int[,] letterToMatrix(string text)
        {
            
            while (text.Length % 3 != 0) text += "а";
            int[,] K = new int[3, text.Length / 3];
            for (int i = 0; i < text.Length; i++)
            {
                K[i % 3, i / 3] = Array.IndexOf(letters, text[i]);
            }
            return K;
        }
        static string matrixToLetter(int[,] array)
        {
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += letters[array[i % 3, i / 3]];
            }
            return text;
        }
        static int algorithmEvklida(int[,] array)
        {
            int K1 = array[0, 0] * array[1, 1] * array[2, 2] +
                array[0, 1] * array[1, 2] * array[2, 0] +
                array[1, 0] * array[2, 1] * array[0, 2];
            int K2 = array[0, 2] * array[1, 1] * array[2, 0] +
                array[0, 1] * array[1, 0] * array[2, 2] +
                array[2, 1] * array[1, 2] * array[0, 0];
            int A = (K1 - K2) % 31, x1 = 1, x2 = 0, y1 = 0, y2 = 1, B = 31, q;
            A += ((A >= 0) ? 0 : 31);
            while (B > 0)
            {
                q = A / B;
                (A, B) = (B, A % B);
                (x1, x2) = (x2, x1 - x2 * q);
                (y1, y2) = (y2, y1 - y2 * q);
            }
            return ((x1 % 31) >= 0 ? x1 % 31 : (x1 % 31) + 31);
        }
        static void printMatrix(int[,] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i / (array.Length / 3), i % (array.Length / 3)]}\t{((i + 1) % (array.Length / 3) == 0 ? "\n" : "")}");
            }
            Console.WriteLine();
        }
        static int[,] matrixMultiply(int[,] X, int[,] Y)
        {
            int[,] K = new int[3, Y.Length / 3];
            for (int i = 0; i < Y.Length; i++)
            {
                int temp = 0;
                for (int j = 0; j < 3; j++)
                {
                    temp += X[i / (Y.Length / 3), j] * Y[j, i % (Y.Length / 3)];
                }
                K[i / (Y.Length / 3), i % (Y.Length / 3)] = temp % 31;
            }
            return K;
        }
        static void fullArray(int[,] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i / 3, i % 3] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();
        }
        static int[,] matrixMultiplyNum(int num, int[,] array)
        {
            int[,] K = new int[3, 3];
            for (int i = 0; i < array.Length; i++)
            {
                K[i / 3, i % 3] = (num * array[i / 3, i % 3]) % 31;
            }
            return K;

        }
        static int[,] detMatrix(int[,] array)
        {
            int[,] K = new int[3, 3];
            for (int i = 0; i < 9; i++)
            {
                int x = i % 3, y = i / 3, index = 0;
                int[] temp = new int[4];
                for (int j = 0; j < 9; j++)
                {
                    if (x != j % 3 && y != j / 3)
                    {
                        temp[index] = array[j / 3, j % 3];
                        index++;
                    }
                }
                K[x, y] = (int)Math.Pow(-1, (x + y)) * (temp[0] * temp[3] - temp[1] * temp[2]) % 31;
                K[x, y] += (K[x, y] >= 0) ? 0 : 31;
            }
            return K;
        }
        static int[,] fullDetMatrix(int[,] Y)
        {
            int num = algorithmEvklida(Y);
            int[,] K = detMatrix(Y);
            Console.WriteLine("Det matrix without multiply");
            printMatrix(K);
            K = matrixMultiplyNum(num, K);
            Console.WriteLine("Det: " + num + "\nFull det matrix");
            printMatrix(K);
            return K;
        }
        static int[,] encryptMatrix(int[,] K, int[,] X)
        {
            return matrixMultiply(K, X);
        }
        static int[,] decryptMatrix(int[,] K, int[,] Y)
        {
            return matrixMultiply(fullDetMatrix(K), Y);
        }
        static int[,] findKey(int[,] Y, int[,] X)
        {
            return matrixMultiply(Y, fullDetMatrix(X));
        }
        static  void Main(string[] args)
        {
            
            int[,] KeyMatrix = letterToMatrix("біопаливо");
            int[,] NameMatrix = letterToMatrix("філіпенкомаксимвалерійовичч");
            int[,] EncryptedMatrix = encryptMatrix(KeyMatrix, NameMatrix);

            Console.WriteLine("Key to matrix");
            printMatrix(KeyMatrix);

            Console.WriteLine(matrixToLetter(EncryptedMatrix));
            printMatrix(EncryptedMatrix);
            
            int[,] DecryptedMatrix = decryptMatrix(KeyMatrix, EncryptedMatrix);

            Console.WriteLine(matrixToLetter(DecryptedMatrix));
            printMatrix(DecryptedMatrix);
            
            printMatrix(findKey(EncryptedMatrix, DecryptedMatrix));
            

            /*
            int[,] Matrix = letterToMatrix("зумiйподи");
            printMatrix(fullDetMatrix(Matrix));
                */

            /*
            // Шифротекст
            int[,] Y = letterToMatrix("філіпенко");
            // Открытый текст
            int[,] X = letterToMatrix("біопаливо");

            Console.WriteLine("Шифротекст (Y)");
            printMatrix(Y);
            Console.WriteLine("Вiдкритий текст(X)");
            printMatrix(X);

            Console.WriteLine("Оберненна матриця (X^-1)");
            printMatrix(fullDetMatrix(X));

            int[,] K = findKey(Y, X);
            Console.WriteLine("Ключ шифрування (К)");
            printMatrix(K);

            Console.WriteLine("Ключ дешифрування (К^-1)");
            printMatrix(fullDetMatrix(K));

            Console.WriteLine("\nПеревiрка");
            printMatrix(encryptMatrix(K, X));
            printMatrix(Y);
            

            
            Console.WriteLine(algorithmEvklida(letterToMatrix("філіпенкомаксимвалерійович")));
            */
        }
    }
}
