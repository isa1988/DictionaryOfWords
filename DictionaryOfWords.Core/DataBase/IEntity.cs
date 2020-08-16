using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}
