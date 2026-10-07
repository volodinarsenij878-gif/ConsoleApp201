using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program201
{
    namespace SpaceRPG_Variant4
    {
        internal class Program
        {
            static void Main()
            {
                // --- Параметры пилота перехватчика ---
                int playerHp = 100;       // Прочность обшивки (основной ресурс)
                int maxHp = 100;
                int shieldEnergy = 50;    // Энергия щитов (вторичный ресурс)
                int maxShield = 50;
                int restoreCount = 3;     // Восстановлений на всю игру

                // --- Волны противников: HP и урон растут ---
                string[] enemyNames = { "Пиратский зонд", "Тяжелый корвет", "Флагманский крейсер" };
                int[] enemyHpArr = { 60, 110, 170 };
                int[] enemyDmgArr = { 10, 16, 22 };

                int totalWaves = 3;
                int currentWave = 0;
                Random rnd = new Random();

                Console.WriteLine("=== КОСМООПЕРА: ПИЛОТ ПЕРЕХВАТЧИКА ===");

                // --- Главный игровой цикл (while): пока есть волны и герой жив ---
                while (currentWave < totalWaves && playerHp > 0)
                {
                    int enemyHp = enemyHpArr[currentWave];
                    int enemyDmg = enemyDmgArr[currentWave];

                    Console.WriteLine($"\n--- Волна {currentWave + 1}: {enemyNames[currentWave]} ---");

                    // Флаг обороны теперь живёт всю волну, а не сбрасывается каждый ход!
                    bool defending = false;

                    // --- Бой с текущим противником (while) ---
                    while (playerHp > 0 && enemyHp > 0)
                    {
                        // Визуализация шкал (for)
                        DrawBar("Обшивка ", playerHp, maxHp, '#', '-');
                        DrawBar("Щиты   ", shieldEnergy, maxShield, '*', '.');
                        Console.WriteLine($"Восстановлений: {restoreCount}");
                        DrawBar("Враг   ", enemyHp, enemyHpArr[currentWave], '█', ' ');
                        Console.WriteLine();

                        // Меню (do-while + switch)
                        int action;
                        bool isValid;
                        do
                        {
                            Console.WriteLine("1 — Базовая атака");
                            Console.WriteLine("2 — Спецспособность (−10 щитов)");
                            Console.WriteLine("3 — Оборона (урон × 0.5)");
                            Console.WriteLine("4 — Восстановить щиты");
                            Console.Write("Выбор: ");

                            isValid = int.TryParse(Console.ReadLine(), out action)
                                      && action >= 1 && action <= 4;

                            if (!isValid)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Ошибка: введите 1–4!\n");
                                Console.ResetColor();
                            }
                        } while (!isValid);

                        // Обработка действий (switch)
                        switch (action)
                        {
                            case 1: // Базовая атака
                                {
                                    int dmg = rnd.Next(15, 23);
                                    enemyHp -= dmg;
                                    Console.WriteLine($"\nВы нанесли {dmg} урона.");
                                    break;
                                }

                            case 2: // Спецспособность
                                {
                                    if (shieldEnergy < 10)
                                    {
                                        Console.WriteLine("\nНедостаточно энергии! Ход пропущен.");
                                        break; // не continue, чтобы не пропускать ход врага
                                    }
                                    shieldEnergy -= 10;
                                    int dmg = rnd.Next(25, 38);
                                    enemyHp -= dmg;
                                    Console.WriteLine($"\nСпецудар: {dmg} урона! Щиты: {shieldEnergy}/{maxShield}.");
                                    break;
                                }

                            case 3: // Оборона
                                {
                                    defending = true;
                                    Console.WriteLine("\nОборонительная стойка активирована.");
                                    break;
                                }

                            case 4: // Восстановление
                                {
                                    if (restoreCount <= 0)
                                    {
                                        Console.WriteLine("\nВосстановления закончились! Ход пропущен.");
                                        break;
                                    }
                                    restoreCount--;
                                    shieldEnergy = Math.Min(maxShield, shieldEnergy + 20);
                                    Console.WriteLine($"\nЩиты восстановлены. Текущая энергия: {shieldEnergy}/{maxShield}. Осталось восстановлений: {restoreCount}.");
                                    break;
                                }
                        }

                        // Ход противника
                        if (enemyHp > 0)
                        {
                            int dmg = enemyDmg + rnd.Next(-2, 3); // лёгкий разброс

                            // Более корректное уменьшение урона (округление вверх)
                            if (defending)
                            {
                                dmg = (dmg + 1) / 2;
                            }

                            playerHp -= dmg;
                            Console.WriteLine($"{enemyNames[currentWave]} наносит {dmg} урона. Обшивка: {Math.Max(0, playerHp)}");
                        }

                        Console.WriteLine();
                    }

                    // Результат волны: сначала проверяем, жив ли игрок
                    if (playerHp <= 0)
                    {
                        break; // игрок погиб — выходим из главного цикла
                    }

                    if (enemyHp <= 0)
                    {
                        Console.WriteLine($"{enemyNames[currentWave]} уничтожен!\n");
                        currentWave++;
                    }
                }

                // --- Итог ---
                Console.WriteLine("=== ИТОГ ===");
                if (currentWave == totalWaves)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Победа! Все противники уничтожены.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Перехватчик уничтожен. Миссия провалена.");
                    Console.ResetColor();
                }
            }

            // Отрисовка шкалы (for)
            static void DrawBar(string label, int current, int max, char fill, char empty)
            {
                Console.Write(label + ": [");
                for (int i = 0; i < current; i++)
                    Console.Write(fill);
                for (int i = 0; i < max - current; i++)
                    Console.Write(empty);
                Console.WriteLine($"] ({current}/{max})");
            }
        }
    }
}