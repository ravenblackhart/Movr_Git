using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicTask : Task
{
    MusicPreference musicPref;

    public override PromptType StartTask(GameManager gameManager)
    {
        //Check for preference in the future
        Debug.Log("Start task, music pref is " + musicPref);
        return PromptType.Main;

        /*if (musicPref == MusicPreference.Jazz) {
            return PromptType.Main;
        }

        else if (musicPref == MusicPreference.House) {
            return PromptType.Secondary;
        }

        else if (musicPref == MusicPreference.Funk) {
            return PromptType.Tertiary;
        }

        else if (musicPref == MusicPreference.Rock) {
            return PromptType
        }

        else if (gameManager.currentCustomer.musicPreference == MusicPreference.House) {
            return PromptType.AddAnotherPromptTypePls;
        }

        else {
            Debug.Log("didn't get music pref?");
            return PromptType.Main;
        }*/
    }

    public override void UpdateTask(GameManager gameManager)
    {
        //Check for the right music tracks later
        if (gameManager.taskReferences.cassettePlayer.audioGenre == "JAZZ" && musicPref == MusicPreference.Jazz) {
            completedTaskEvent.Invoke();
        }

        else if (gameManager.taskReferences.cassettePlayer.audioGenre == "HOUSE" && musicPref == MusicPreference.House) {
            completedTaskEvent.Invoke();
        }

        else if (gameManager.taskReferences.cassettePlayer.audioGenre == "FUNK" && musicPref == MusicPreference.Funk) {
            completedTaskEvent.Invoke();
        }

        else if (gameManager.taskReferences.cassettePlayer.audioGenre == "ROCK" && musicPref == MusicPreference.Rock) {
            completedTaskEvent.Invoke();
        }
    }

    public override void EndTask(GameManager gameManager)
    {
        //
    }

    public override bool CheckValid(GameManager gameManager)
    {
        musicPref = gameManager.currentCustomer.musicPreference;

        if (musicPref == MusicPreference.Any) {
            musicPref = (MusicPreference)Random.Range(1, 4);
            Debug.Log("RANDOM MUSIC PREF = " + musicPref);
        }

        if (CheckMusicPlayer(gameManager) == "JAZZ" && musicPref == MusicPreference.Jazz) {
            return false;
        }

        else if (CheckMusicPlayer(gameManager) == "FUNK" && musicPref == MusicPreference.House) {
            return false;
        }

        else if (CheckMusicPlayer(gameManager) == "HOUSE" && musicPref == MusicPreference.Funk) {
            return false;
        }

        else if (CheckMusicPlayer(gameManager) == "ROCK" && musicPref == MusicPreference.Rock) {
            return false;
        }


        else {
            return true;
        }
    }

    private string CheckMusicPlayer(GameManager gameManager) {
       
        return gameManager.taskReferences.cassettePlayer.audioGenre;
    }
}
