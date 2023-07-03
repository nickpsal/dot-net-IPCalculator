namespace IPCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunApp();
        }

        private static void RunApp()
        {
            Console.WriteLine("-".PadLeft(92,'-').PadRight(92,'-'));
            Console.WriteLine("|                                   IP Calculator V1                                       |");
            Console.WriteLine("|                                       (C) 2023                                           |");
            Console.WriteLine("-".PadLeft(92, '-').PadRight(92, '-'));
            Console.WriteLine("|            Πληκτρολογέις στην εφαρμογή την διεύθηνση IP που σεν ενδιαφέρει               |");
            Console.WriteLine("|        για να υπολογίσεις την subnet mask και δινεις τον αριθμό των υποδικτύων           |");
            Console.WriteLine("|                          ή των host που θες να σπάσει το δίκτυο                          |");
            Console.WriteLine("-".PadLeft(92, '-').PadRight(92, '-'));
            int[] IPint = checkIP();
            string SubnetMask = CalculateSubnetMask(IPint[0]);
            Console.WriteLine("Η διευθηνση IP {0} έχει subnet mask {1}", IPint[0] + "." + IPint[1] + "." + IPint[2] + "." + IPint[3], SubnetMask);
            CalculateNetworkandHost(SubnetMask);
            Console.Read();
        }

        private static int[] checkIP()
        {
            int[] IntIP = new int[4];
            while (true)
            {
                int count = 0;
                Console.WriteLine("Πληκτρολόγησε την IP που σε ενδιαφέρει:");
                string? IPString = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(IPString))
                {
                    Console.WriteLine("Δώσατε κενή Επιλογή");
                    continue;
                }
                string[] IPtoArray = IPString.Split('.');
                if (IPtoArray.Length != 4)
                {
                    Console.WriteLine("Μη έγκυρη διεύθυνση IP. Παρακαλώ πληκτρολογήστε 4 αριθμούς χωρισμένους με τελείες.");
                    continue;
                }
                foreach (string str in IPtoArray)
                {
                    if (int.TryParse(str, out int parsedValue) && parsedValue >=0 && parsedValue <=255)
                    {
                        IntIP[count++] = parsedValue;
                    }else
                    {
                        Console.WriteLine("Δώσατε Λανθασμένη διεύθηνση IP");
                        IntIP[count] = 0;
                        count = 0;
                        break;
                    }
                }
                if (count == 4)
                {
                    break;
                }
            }
            return IntIP;
        }

        private static string CalculateSubnetMask(int IPint)
        {
            string binary = Convert.ToString(IPint,2).PadLeft(8,'0');
            if (binary[..2] == "11")
            {
                return "255.255.255.0";
            }else if (binary[..2] == "10")
            {
                return "255.255.0.0";
            }
            return "255.0.0.0";
        }

        private static void CalculateNetworkandHost(string SubnetMask)
        {
            while (true)
            {
                Console.WriteLine("Θέλετε να σπάσετε το δίκτυο με βάση");
                Console.WriteLine("s : πόσα υποδίκτυα θέλετε???");
                Console.WriteLine("h : με βάση πόσους host θελετε σε κάθε υποδίκτυο???");
                Console.WriteLine("Κάντε την Επιλογή σας");
                string? epilogi = Console.ReadLine();
                if (epilogi == "s" || epilogi == "S")
                {
                    CalculatebyNetwork(SubnetMask);
                    break;
                }else if (epilogi == "h" || epilogi == "H")
                {
                    CalculatebyHost(SubnetMask);
                    break;
                }else
                {
                    Console.WriteLine("Έδωσες Λάθος Επιλογή");
                    continue;
                }
            }
        }

        private static void CalculatebyNetwork(string SubnetMask)
        {
            int subnets = 0;
            int bits = 0;
            while (true) {
                Console.WriteLine("Πόσα Υποδικτύα θέλετε απο 2 και πανω???");
                string? epilogi = Console.ReadLine();
                if (epilogi != null && int.TryParse(epilogi, out subnets) && subnets >= 2)
                {
                    break;
                }
                Console.WriteLine("Δώσατε Κενή επιλογή ή δεν δώσατε αριθμό");
            }
            Console.WriteLine("Θέλουμε {0}",subnets);
            for (int i = 0; i<=8; i++)
            {
                if (Math.Pow(2,i) >= subnets)
                {
                    bits = i;
                    break;
                }
            }
            Console.WriteLine("Για να δημιουργήσουμε {0} subnets χρειαζόμαστε {1} bits",subnets,bits);
            CalculateNewSubnetMask(SubnetMask, bits);
        }

        private static void CalculatebyHost(string SubnetMask)
        {
            int hosts = 0;
            int bits = 0;
            while (true)
            {
                Console.WriteLine("Πόσα hosts θέλετε απο 2 και πανω???");
                string? epilogi = Console.ReadLine();
                if (epilogi != null && int.TryParse(epilogi, out hosts) && hosts >=2)
                {
                    break;
                }
                Console.WriteLine("Δώσατε Κενή επιλογή ή δεν δώσατε αριθμό");
            }
            Console.WriteLine("Θέλουμε {0}", hosts);
            for (int i = 0; i <= 8; i++)
            {
                if (Math.Pow(2, i) >= hosts)
                {
                    bits = i;
                    Console.WriteLine(i);
                    break;
                }
            }
            Console.WriteLine("Για να δημιουργήσουμε {0} hosts για κάθε υποδίκτυο χρειαζόμαστε χρειαζόμαστε {1} bits", hosts, bits);
            int remainbits = 8 - bits;
            CalculateNewSubnetMask(SubnetMask, remainbits);
        }

        private static void CalculateNewSubnetMask(string SubnetMask, int bits)
        {
            string binary = "";
            for (int i = 0; i < bits; i++)
            {
                binary += 1;
            }
            binary = binary.PadRight(8, '0');
            string mask = (Convert.ToInt32(binary, 2) - 1).ToString();
            string newSubnetMask = SubnetMask.Replace("0", mask);
            Console.WriteLine("Η Νέα Subnet Mask για την υποδικτύωση είναι η {0}", newSubnetMask);
        }
    }
}