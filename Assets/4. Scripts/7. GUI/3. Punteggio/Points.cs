using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    #region Attributes

    public TextMeshProUGUI pointsCollected;
    private int _points;
    private float _finalScore;

    #endregion


    private void OnEnable()
    {
        //pointsCollected.text = PlayerController.punteggioTotale.ToString();
        //nuova versione con la Coroutine la seguente linea di codice viene disattivata
        //pointsCollected.SetText(PlayerController.punteggioTotale.ToString());

        //nuova versione
        StartCoroutine(AnimateText());
        //StartCoroutine(AnimateFinalScore());
    }

    IEnumerator AnimateText()
    {

        pointsCollected.SetText("Points:");
        yield return new WaitForSeconds(.9f);

        pointsCollected.SetText("0");
        //_points = 0;

        //aggiungere attesa affinche il calcolo si veda bene che parta da zero
        yield return new WaitForSeconds(.9f);

        //sostituire 10 con numero di punti totalizzato dal giocatore
        /*
        while (_points < PlayerController.PunteggioTotale) //sostituire il playercontroller con il game manager
        {
            _points++;
            pointsCollected.SetText(_points.ToString());
            //aggiungere un attesa per dare tempo al giocatore di vedere il numeratore che cresce
            yield return new WaitForSeconds(.1f);
        }
        */

        //Aggiungo il tempo al punteggio animandolo

        _finalScore = GameManager.Points + GameManager.ExtraPoints;
        yield return new WaitForSeconds(.9f);

        pointsCollected.SetText(" + Time");
        yield return new WaitForSeconds(.9f);

        pointsCollected.SetText(((int)Timer.CurrentTime).ToString());
        yield return new WaitForSeconds(.1f);

        pointsCollected.SetText(_finalScore.ToString());
    }

    /*
    IEnumerator AnimateFinalScore()
    {
        float finalScore = 0;

        finalScore = _points + GameManager.extraPoints;
        yield return new WaitForSeconds(.2f);

        pointsCollected.SetText(finalScore.ToString());
    }
    */
}
