namespace Sdcb.LibRaw.Indexers;

internal class GammaIndexer(IntPtr r, bool disposed) : IIndexer<float>
{
    private readonly IntPtr _r = r;
    private readonly bool _disposed = disposed;

    /// <seealso cref="LibRawNative.SetGamma(IntPtr, int, float)"/> 
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
            return (float)data.OutputParams.Gamma[index];
        }
        set
        {
            this.CheckDisposed();
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            LibRawNative.SetGamma(this._r, index, value);
        }
    }

    public int Count => 2;

    public IEnumerator<float> GetEnumerator()
    {
        for (int i = 0; i < this.Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    private void CheckDisposed()
        => ObjectDisposedException.ThrowIf(this._disposed, this);
}