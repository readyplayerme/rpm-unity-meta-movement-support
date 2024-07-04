using System;
using System.Net.Http;
using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

public class GenderCheck 
{
    private string url;

    public GenderCheck(string url)
    {
        this.url = url;
    }

    public async Task<OutfitGender> CheckGenderAsync()
    {
        try
        {
            using HttpClient client = new HttpClient();
            
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            AvatarMetadata characterData = JsonUtility.FromJson<AvatarMetadata>(jsonResponse);
            return characterData.OutfitGender;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching JSON data: {ex.Message}. Defaulting to masculine.");
            return OutfitGender.Masculine;
        }
    }
    
}
