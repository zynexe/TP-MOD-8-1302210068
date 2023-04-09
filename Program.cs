

using System;
using System.IO;
using Newtonsoft.Json;

public class CovidConfig
{
    public string SatuanSuhu { get; set; }
    public int BatasHariDeman { get; set; }
    public string PesanDitolak { get; set; }
    public string PesanDiterima { get; set; }
}

public class CovidConfigurator
{
    private const string CONFIG_FILE = "covid_config.json";
    private CovidConfig config;

    public CovidConfigurator()
    {
        LoadConfig();
    }

    public void LoadConfig()
    {
        if (File.Exists(CONFIG_FILE))
        {
            string json = File.ReadAllText(CONFIG_FILE);
            config = JsonConvert.DeserializeObject<CovidConfig>(json);
        }
        else
        {
            config = new CovidConfig
            {
                SatuanSuhu = "celcius",
                BatasHariDeman = 14,
                PesanDitolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini",
                PesanDiterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini"
            };
        }
    }

    public string GetPesanDiterima()
    {
        string pesan = config.PesanDiterima;
        if (config.SatuanSuhu == "celcius")
        {
            double suhu = GetInputSuhu();
            if (suhu < 36.5 || suhu > 37.5)
            {
                pesan = config.PesanDitolak;
            }
        }
        else if (config.SatuanSuhu == "fahrenheit")
        {
            double suhu = GetInputSuhu();
            if (suhu < 97.7 || suhu > 99.5)
            {
                pesan = config.PesanDitolak;
            }
        }
        int hariDeman = GetInputHariDeman();
        if (hariDeman >= config.BatasHariDeman)
        {
            pesan = config.PesanDitolak;
        }
        return pesan;
    }

    private double GetInputSuhu()
    {
        Console.Write($"Berapa suhu badan anda saat ini? Dalam nilai {config.SatuanSuhu}: ");
        double suhu = double.Parse(Console.ReadLine());
        return suhu;
    }

    private int GetInputHariDeman()
    {
        Console.Write("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala deman? ");
        int hariDeman = int.Parse(Console.ReadLine());
        return hariDeman;
    }

    public void UbahSatuan()
    {
        if (config.SatuanSuhu == "celcius")
        {
            config.SatuanSuhu = "fahrenheit";
        }
        else if (config.SatuanSuhu == "fahrenheit")
        {
            config.SatuanSuhu = "celcius";
        }
        string json = JsonConvert.SerializeObject(config);
        File.WriteAllText(CONFIG_FILE, json);
        Console.WriteLine($"Satuan suhu berhasil diubah menjadi {config.SatuanSuhu}");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        CovidConfigurator configurator = new CovidConfigurator();
        Console.WriteLine(configurator.GetPesanDiterima());

        configurator.UbahSatuan();
        Console.WriteLine(configurator.GetPesanDiterima());
    }
}

