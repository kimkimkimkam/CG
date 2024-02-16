using System.Collections;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    // Prefab for the background particle
    public GameObject particlePrefab;

    // Number of particles per row and column
    public int numRows = 10;
    public int numColumns = 30;

    // Range for the particle size
    public float minSize = 0.5f;
    public float maxSize = 5.0f;

    // Audio source for music playback
    public AudioVisualisation audioV;

    // Scale factor for particle size based on music amplitude
    public float sizeScale = 10f;

    // Delay between rows and columns
    public float rowDelay = 0.1f;
    public float columnDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Generate particles in a grid
        GenerateParticleGrid();
    }

    // Generate particles in a grid
    public void GenerateParticleGrid()
    {
        float xOffset = (numColumns - 1) * 0.5f;
        float yOffset = (numRows - 1) * 0.5f;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // Calculate position for the particle
                float x = col - xOffset;
                float y = row - yOffset;
                Vector3 position = new Vector3(x, y, 0f);

                // Instantiate particle
                GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity, transform);

                // Random size for the particle
                float size = Random.Range(minSize, maxSize);

                // Set particle size
                particle.transform.localScale = new Vector3(size, size, size);
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // Update particle sizes based on music amplitude with delayed rows and columns
        UpdateParticleSizesWithDelay();
    }

    // Update particle sizes based on music amplitude with delayed rows and columns
    void UpdateParticleSizesWithDelay()
    {
        // Get current music amplitude
        float amplitude = GetMusicAmplitude();

        // Scale particle sizes based on music amplitude with delayed rows and columns
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // Calculate index and delay for the particle
                int index = row * numColumns + col;
                float delay = row * rowDelay + col * columnDelay;

                // Delay the size update based on the particle's position
                UpdateParticleSizeDelayed(index, amplitude, delay);
            }
        }
    }

    // Delayed update of particle size based on music amplitude
    void UpdateParticleSizeDelayed(int index, float amplitude, float delay)
    {
        // Delay the size update based on the particle's position
        StartCoroutine(DelayedSizeUpdate(index, amplitude, delay));
    }

    // Coroutine for delayed size update of particle
    IEnumerator DelayedSizeUpdate(int index, float amplitude, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Find the particle at the specified index
        Transform particle = transform.GetChild(index);
        if (particle != null)
        {
            // Scale particle size based on music amplitude
            float newSize = Mathf.Lerp(minSize, maxSize, amplitude * sizeScale);
            particle.localScale = new Vector3(newSize, newSize, newSize);
        }
    }

    // Get the current music amplitude
    float GetMusicAmplitude()
    {
        float[] spectrumData = new float[256];
        audioV.GetCurrentSong().GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Calculate average amplitude from spectrum data
        float sum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            sum += spectrumData[i];
        }
        float averageAmplitude = sum / spectrumData.Length;

        return averageAmplitude;
    }
}
