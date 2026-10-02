namespace Sdcb.LibRaw.Indexers;

internal class UserMultiplierIndexer : IIndexer<float>
{
    private readonly IntPtr _r;
    private readonly bool _disposed;

    public UserMultiplierIndexer(IntPtr r, bool disposed)
    {
        this._r = r;
        this._disposed = disposed;
    }

    /// <seealso cref="LibRawNative.SetUserMultiplier"/> 
    /// <seealso cref="LibRawData"/>
    public float this[int index]
    {
        get
        {
            this.CheckDisposed();
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            LibRawData data = Marshal.PtrToStructure<LibRawData>(this._r);
            return data.OutputParams.UserMultipliers[index];
        }
        set
        {
            this.CheckDisposed();
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            LibRawNative.SetUserMultiplier(this._r, index, value);
        }
    }

    public int Count => 4;

    public IEnumerator<float> GetEnumerator()
    {
        for (int i = 0; i < this.Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    private void CheckDisposed()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(RawContext));
        }
    }
}