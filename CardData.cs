using System;
using Unity.Netcode;

public enum CardType : byte
{
    Letter = 0,
    Wildcard = 1,
    LoseTurn = 2
}

// Simple serializable card data for Netcode
public struct CardData : INetworkSerializable, IEquatable<CardData>
{
    public CardType Type;
    public char Letter; // uppercase A-Z used when Type==Letter; ignored otherwise

    public CardData(CardType type, char letter)
    {
        Type = type;
        Letter = letter;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Type);
        serializer.SerializeValue(ref Letter);
    }

    public bool Equals(CardData other)
    {
        return Type == other.Type && Letter == other.Letter;
    }

    public override string ToString()
    {
        if (Type == CardType.Letter) return Letter.ToString();
        if (Type == CardType.Wildcard) return "*";
        return "LoseTurn";
    }
}
