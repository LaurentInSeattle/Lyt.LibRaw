namespace Sdcb.LibRaw;

/// <summary>Represents a processed image.</summary>
public unsafe class ProcessedImage : IDisposable
{
    private LibRawProcessedImage* _image;

    /// <summary>Initializes a new instance of the <see cref="ProcessedImage"/> class.</summary>
    /// <param name="image">The LibRaw processed image.</param>
    public ProcessedImage(LibRawProcessedImage* image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        this._image = image;
        GC.AddMemoryPressure(this._image->DataSize);
    }

    /// <summary>Gets a value indicating whether this instance has been disposed.</summary>
    public bool Disposed => this._image == null;

    /// <summary>Gets the image format.</summary>
    public ProcessedImageType ImageType => this._image->Type;

    /// <summary>Gets the image height.</summary>
    public int Height => this._image->Height;

    /// <summary>Gets the image weight.</summary>
    public int Width => this._image->Width;

    /// <summary>Gets the number of color channels in the image.</summary>
    public int Channels => this._image->Colors;

    /// <summary>Gets the bits value of the image.</summary>
    public int Bits => this._image->Bits;

    /// <summary>Gets the image data length in byte.</summary>
    public int DataSize => this._image->DataSize;

    /// <summary>Gets the pointer to the first byte of the image data.</summary>
    public IntPtr DataPointer => (IntPtr)(&this._image->FirstData);

    /// <summary>Gets the data as an array-like <see cref="Span{T}"/> object.</summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>The data in a <see cref="Span{T}"/> object.</returns>
    public Span<T> AsSpan<T>() => this._image->GetData<T>();

    /// <summary>Swaps the red and blue bytes of the image data of a bitmap image.</summary>
    /// <exception cref="InvalidOperationException">Throw when <see cref="ImageType"/> is not <see cref="ProcessedImageType.Bitmap"/></exception>
    public unsafe void SwapRGB()
    {
        if (this.ImageType != ProcessedImageType.Bitmap)
        {
            throw new InvalidOperationException($"Only bitmap image can be swapped.");
        }

        byte* from = &this._image->FirstData;
        byte* to = from + this._image->Width * this._image->Height * 3;
        for (byte* ptr = from; ptr < to; ptr += 3)
        {
            byte red = ptr[0];
            byte blue = ptr[2];

            ptr[0] = blue;
            ptr[2] = red;
        }
    }

    #region Dispose Pattern

    /// <summary>Allows the finalizer to free memory used by this instance.</summary>
    ~ProcessedImage()
    {
        this.Dispose(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Cleanup the managed and unmanaged resources used.</summary>
    /// <param name="disposing">Flag indicating if <see cref="_image"/> should be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.Disposed)
        {
            if (this._image != null)
            {
                this.FreeImage();
            }
        }
    }

    /// <summary>Frees the memory used by this instance.</summary>
    private void FreeImage()
    {
        int size = this._image->DataSize;
        LibRawNative.ClearDcrawMemory((IntPtr)this._image);
        GC.RemoveMemoryPressure(size);
        this._image = null;
    }

    #endregion
}
