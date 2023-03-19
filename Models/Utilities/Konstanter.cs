// KONSTANTER MED MERA GEMENSAMMA FÖR HELA APPLIKATIONEN;

using System;
using System.Collections.Generic;
using Projekt.Models;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Kategorier som tillhör MEDIA;
/// Dessa är bok, CD, DVD m.m.
/// </summary>
[Serializable]
public enum Kategori
{
    None = 0, Bok = 1, CD = 2, DVD = 3
}

[Serializable]
public enum Film
{
    None = 0, Spelfilm = 1, Dokumentär = 2, Video = 3, Allt = 4
}

[Serializable]
public enum Litteratur
{
    None = 0, Skön = 1, Fack = 2, Fakta = 3, Allt = 4
}

[Serializable]
public enum Musik
{
    None = 0, Modern = 1, Klassisk = 2, Audio = 3, Allt = 4
}

[Serializable]
public enum DVDTyp
{
    Film, Video, Spel
}

[Serializable]
public enum CDTyp
{
    Musik, Audio, Ljudbok, Språkkurs
}

/* Enum MOMSSATSER tillåter ENBART SVENSKA momssatser:
 * 0% (för momsbefriade varor och tjänster), 6%, 12% och 25%; 
 * Applikationen accepterar INTE andra momssatser än dessa.
 */

[Serializable]
public enum MOMSSATSER
{
    ZERO = 0,
    SEX = 6,
    TOLV = 12,
    TJUGOFEM = 25
};

[Serializable]
public enum Land
{
    Australien,
    Belgien,
    Danmark,
    England,
    Estland,
    Finland,
    Frankrike,
    Grekland,
    Holland,
    Irland,
    Island,
    Italien,
    Kanada,
    Lettland,
    Litauen,
    Norge,
    Polen,
    Portugal,
    Ryssland,
    Schweiz,
    Spanien,
    Sverige,
    Tjeckien,
    Turkiet,
    Tyskland,
    Ukraina,
    USA
}

[Serializable]
public class FullNameAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString())
            || string.IsNullOrWhiteSpace(value.ToString()))
            return false;
        else
        {
            var nameComponents = value.ToString().Split(' ');
            return nameComponents.Length >= 2;
        }
    }
}

[Serializable]
public class PrisIntegerAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString())
            || string.IsNullOrWhiteSpace(value.ToString()))
            return false;
        else
        {
            string input = value.ToString();
            return int.TryParse(input, out int pris);
        }
    }
}

/// <summary>
///  Sorterar kunder i nummerisk ordning;
/// </summary>

[Serializable]
public class SortCustomer : IComparer<Customer>
{
    public int Compare(Customer a, Customer b)
    {
        return a.Nummer.CompareTo(b.Nummer);
    }
}

[Serializable]
public class SortEmpl : IComparer<Employee>
{
    public int Compare(Employee a, Employee b)
    {
        return a.Nummer.CompareTo(b.Nummer);
    }
}

[Serializable]
public class SortMedia : IComparer<Media>
{
    public int Compare(Media a, Media b)
    {
        if (a.Number == b.Number)
            return a.Titel.CompareTo(b.Titel);
        else
            return a.Number.CompareTo(b.Number);
    }
}

[Serializable]
public class SortBooks : IComparer<Book>
{
    public int Compare(Book a, Book b)
    {
        return a.Nummer.CompareTo(b.Nummer);
    }
}

[Serializable]
public class SortCDs : IComparer<Cd>
{
    public int Compare(Cd a, Cd b)
    {
        return a.Nummer.CompareTo(b.Nummer);
    }
}

[Serializable]
public class SortDVDs : IComparer<Dvd>
{
    public int Compare(Dvd a, Dvd b)
    {
        return a.Nummer.CompareTo(b.Nummer);
    }
}