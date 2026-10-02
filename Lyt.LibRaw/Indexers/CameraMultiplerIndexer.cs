namespace Sdcb.LibRaw.Indexers;

internal class CameraMultiplerIndexer : IReadOnlyList<float>
{
    private readonly IntPtr _r;
    private readonly bool _disposed;

    public CameraMultiplerIndexer(IntPtr r, bool disposed)
    {
        this._r = r;
        this._disposed = disposed;
    }

    /// <summary>Gets the white balance coefficients for the camera.</summary>
    /// <seealso cref="LibRawNative.GetCameraMultiplier(IntPtr, int)"/> 
    public float this[int index]
    {
        get
        {
            this.CheckDisposed();
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            return LibRawNative.GetCameraMultiplier(this._r, index);
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