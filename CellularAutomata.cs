using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    public int maxInitialDepth = 5; // Initial maximum depth of recursion
    public int maxDepthIncrease = 2; // Maximum depth increase over time
    public int width = 50; // Width of the cellular automata grid
    public float branchInterval = 0.5f; // Interval between adding new branches (adjust for faster respawn)
    public float trunkLength = 5f; // Length of the trunk
    public float trunkWidth = 0.5f; // Width of the trunk
    public float branchLengthFactor = 0.7f; // Length factor for child branches
    public float branchWidthFactor = 0.7f; // Width factor for child branches

    public float angle = 22f; // Angle between branches
    public GameObject branchPrefab; // Prefab for branches
    public GameObject boundary; // Empty GameObject serving as the boundary

    private int currentDepth;
    private Transform lastBranch;

    void Start()
    {
        currentDepth = maxInitialDepth;

        // Create the initial trunk
        CreateBranch(Vector3.zero, Vector3.up * trunkLength, transform.rotation, 0, maxInitialDepth);
        lastBranch = transform;

        // Start the continuous growth process
        InvokeRepeating(nameof(AddNewBranch), 0f, branchInterval); // Start immediately and respawn branches faster
    }

    void Update()
    {
        //Debug.Log((AudioReaction._spectrum[2] * 100000f));
        Debug.Log((AudioReaction._spectrum[0] * 100f));
    }

    void CreateBranch(Vector3 startPos, Vector3 direction, Quaternion rotation, int depth, int maxDepth)
    {
        // Create the branch GameObject
        GameObject branch = Instantiate(branchPrefab, startPos, rotation, transform);

        // Set branch length and width
        float length = direction.magnitude;
        branch.transform.localScale = new Vector3((trunkWidth + (AudioReaction._spectrum[0] * 100f)), length, (trunkWidth + (AudioReaction._spectrum[0] * 100f)));
        branch.transform.localPosition += direction * 0.5f;

        // Set branch rotation
        branch.transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);

        // Check if the branch is outside the boundary
        if (Vector3.Distance(branch.transform.position, boundary.transform.position) > boundary.transform.localScale.x / 2)
        {
            // Destroy the branch and spawn a new one at the center
            Destroy(branch);
            CreateBranch(Vector3.zero, Vector3.up * trunkLength, transform.rotation, 0, maxInitialDepth);
            return;
        }

        // Check recursion depth
        if (depth < maxDepth)
        {
            // Create child branches
            Vector3 newPosition = startPos + direction;
            Vector3 newDirection1 = Quaternion.AngleAxis(Random.Range((-angle + (AudioReaction._spectrum[2] * 100000f)), (angle + (AudioReaction._spectrum[2] * 100000f))), Vector3.forward) * direction * branchLengthFactor;
            Vector3 newDirection2 = Quaternion.AngleAxis(Random.Range((-angle + (AudioReaction._spectrum[2] * 100000f)), (angle + (AudioReaction._spectrum[2] * 100000f))), Vector3.forward) * direction * branchLengthFactor;
            Quaternion newRotation1 = Quaternion.AngleAxis(Random.Range((-angle + (AudioReaction._spectrum[2] * 100000f)), (angle + (AudioReaction._spectrum[2] * 100000f))), Vector3.up) * rotation;
            Quaternion newRotation2 = Quaternion.AngleAxis(Random.Range((-angle + (AudioReaction._spectrum[2] * 100000f)), (angle + (AudioReaction._spectrum[2] * 100000f))), Vector3.up) * rotation;
            CreateBranch(newPosition, newDirection1, newRotation1, depth + 1, maxDepth);
            CreateBranch(newPosition, newDirection2, newRotation2, depth + 1, maxDepth);
        }
    }

    void AddNewBranch()
    {
        // Increase depth
        if (currentDepth < maxInitialDepth + maxDepthIncrease)
        {
            currentDepth++;
        }

        // Add a new branch to the last branch created
        Vector3 newDirection = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * lastBranch.up * branchLengthFactor;
        Quaternion newRotation = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.up) * lastBranch.rotation;
        CreateBranch(lastBranch.position, newDirection, newRotation, 0, currentDepth);

        // Update last branch reference
        lastBranch = transform.GetChild(transform.childCount - 1);
    }
}