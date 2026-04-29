using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ApplicantSO applicantSO;
    public JobPostingSO jobPostingSO;
    public GameObject resumePrefab;
    public Transform canvasContainer;
    public void InitializeApplicantResume()
    {
        // Debug all the information of the applicant
        Debug.Log(applicantSO.applicantName);
        Debug.Log(applicantSO.funFact);

        // Instantiate the resume prefab and set the data
        var resumeGO = Instantiate(resumePrefab, canvasContainer);
        var resumeScript = resumeGO.GetComponent<ResumeScript>();
        resumeScript.SetResumeData(applicantSO);

    }

    public void AcceptCurrentApplicant()
    {
        List<ApplicantTrait> applicantTraits = GetApplicantTraits();
        bool meetsRequirements = true;

        // Check if applicant has all required traits
        foreach (var requiredTrait in jobPostingSO.requiredTraits)
        {
            if (!applicantTraits.Contains(requiredTrait))
            {
                meetsRequirements = false;
                break;
            }
        }

        // Check if applicant has any disqualifying traits
        foreach (var disqualifyingTrait in jobPostingSO.disqualifyingTraits)
        {
            if (applicantTraits.Contains(disqualifyingTrait))
            {
                meetsRequirements = false;
                break;
            }
        }

        if (meetsRequirements)
        {
            Debug.Log("Applicant Accepted: " + applicantSO.applicantName);
        }
        else
        {
            Debug.Log("Applicant Rejected: " + applicantSO.applicantName);
        }
    }

    public void RejectCurrentApplicant()
    {
        Debug.Log("Applicant Rejected: " + applicantSO.applicantName);
    }

    private List<ApplicantTrait> GetApplicantTraits()
    {
        List<ApplicantTrait> traits = new List<ApplicantTrait>();

        foreach (var job in applicantSO.jobExperiences)
        {
            traits.Add(job.trait);
        }

        foreach (var project in applicantSO.personalProjects)
        {
            traits.Add(project.trait);
        }

        foreach (var education in applicantSO.education)
        {
            traits.Add(education.trait);
        }

        traits.Add(applicantSO.funFact.trait);

        return traits;
    }
}
