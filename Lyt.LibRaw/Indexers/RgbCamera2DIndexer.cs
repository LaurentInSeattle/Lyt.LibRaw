namespace Sdcb.LibRaw.Indexers;

internal class RgbCamera2DIndexer : IReadOnly2DIndexer<float>
{
    private readonly IntPtr _r;
    private readonly bool _disposed;

    public RgbCamera2DIndexer(IntPtr r, bool disposed)
    {
        this._r = r;
        this._disposed = disposed;
    }

    /// <seealso cref="LibRawNative.GetRgbCameraMatrix(IntPtr, int, int)"/>
    public float this[int index]
    {
        get
        {
            this.CheckDisposed();
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            int x = index % this.Width;
            int y = index / this.Width;
            return LibRawNative.GetRgbCameraMatrix(this._r, y, x);
        }
    }

    /// <seealso cref="LibRawNative.GetRgbCameraMatrix(IntPtr, int, int)"/>
    public float this[int y, int x]
    {
        get
        {
            this.CheckDisposed();
            if (y < 0 || y >= this.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y), "Index is out of range.");
            }

            if (y < 0 || y >= this.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "Index is out of range.");
            }

            return LibRawNative.GetRgbCameraMatrix(this._r, y, x);
        }
    }

    public int Count => this.Width * this.Height;
    public int Width => 4;
    public int Height => 3;

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