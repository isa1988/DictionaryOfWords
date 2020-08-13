using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class Language : EntityBase
    {
        public Language()
        {
            // Я бы не делал значение по умолчанию.
            // 1. При создании инстанса в твоем коде клиенты твоего кода не сразу поймут, что Имя заполняется на пустую строку.
            // 2. При инициации инстанса из EF Core все равно имя будет перезаписано тем, что содержится в базе, а ам может быть и null.
            // так ты, в итоге, не обезопасил себя от Name = null на выходе из DbSet-ов, а в своем коде сервисов/контоллеров создал
            // неочевидное поведение класса
            // Если хочется сделать заполнение поля name, то лучше это сделать через конструктор типа
            //
            // public Language(string name = "")
            Name = string.Empty;
            Words = new List<Word>();
            WordFromTranslations = new List<WordTranslation>();
            WordToTranslations = new List<WordTranslation>();
        }
        public string Name { get; set; }

        // МОжно интерфейс ICollection поставить здесь. Тогда ты сможешь в своем коде при подаче инстанса в DbSet проставлять не только
        // список List, но и массив Array
        public virtual ICollection<Word> Words {get; set; }

        public virtual List<WordTranslation> WordFromTranslations { get; set; }

        public virtual List<WordTranslation> WordToTranslations { get; set; }

    }
}
