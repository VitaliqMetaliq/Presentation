﻿namespace Storage.Main.Models;

public class DailyCurrencyViewModel
{
    public int Id { get; set; }
    public string CurrencyCode { get; set; }
    public string TargetCurrencyCode { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
}
