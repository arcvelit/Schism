using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class ProgressGlobal : MonoBehaviour
{
    public static ProgressGlobal Instance { get; private set; }

    private static string ERROR_SCROLL = "";
    private static int deposited;
    private static HashSet<int> booksCollected = new HashSet<int>();
    private static HashSet<int> difficultyLevels = new HashSet<int> { 2, 5, 7, 10 };
    private static Dictionary<int, string> scrolls = new Dictionary<int, string> { 
        { 1,  "Trip to New York\nJuly 16th, 1983\n\nI've always loved small towns, and Hollowstone is no exception. There's a charm here-a simplicity that feels untouched by time. The main road though is in terrible shape-cracked asphalt, overgrown weeds, and potholes deep enough to break ankles. It's the kind of road that makes you feel like you are deep in the middle of nowhere.\n\nI arrived in Hollowstone expecting little more than a quiet stopover, but the people here live like time forgot them-simple, trusting, and unshaken by the outside world. However, something about this place feels off. The streets are too quiet, even for a town like this. Just the wind, and something else I can't quite put my finger on.\n\nI passed by a field just next to the road. There was a gathering, or so I thought. People were in a circle, all facing a man stood on a hay bale. He spoke with a voice that carried far too well for someone speaking so softly. It wasn't just the words-though I couldn't make out most of them-it was the way people looked at him, as if he held their lives in his han-\n\n\n*Page is ripped*" }, 
        { 2,  "*Page starts here*\n\nstood there, under the open sky, as he spoke of storms to come and dangers unseen, promising salvation through relics only he could possess.\n\nThe townsfolk were skeptical at first, but the stranger had a way of speaking that silenced doubt. When the crops began to wither without explanation, when cattle fell ill despite their care, desperation took hold. A small group gathered here in the field, under a cold, cloudless sky, to hear his sermons. His words were a balm to their fears, and soon, he had them performing rituals. Small ones, at first-offering grain and murmured prayers. \n\nThe stranger promised the relics he possessed would protect them, and the group grew bigger. But still not all believed him. \n\nHaving faith that brighter times would come, some families chose to abstain from the rituals. The Nelsons were the last family to actively disavow the man's preaches. Their house at a secluded end of the streets near the field. Perhaps some answers lie there, though I doubt you'll find solace within its walls." }, 
        { 3,  "The preacher stayed with us for months, and we welcomed him like family. He chose this house as the first to receive his so-called blessing, the 'relic' that would shield us from evil. We hung it proudly above the door-a carved symbol, ancient and strange. He said it came from a forgotten faith, a lost knowledge that only he had preserved.\n\nAt first, the house felt warmer, more peaceful. But then the whispers began. Faint at first, just at the edge of hearing. By the second month, my wife swore she could hear them inside her dreams. Our youngest, 'liz, became pale and withdrawn. She stopped playing in the yard, and when I asked why, she just said, \n\n'They're watching from the field.'\n\nOne night, I found Elizabeth sitting by the window, staring out at the dark. She didn't move, didn't speak, didn't even blink, even when I shook her. When she finally spoke, she whispered,\n\n'We shouldn't have let him in. These relics, they aren't saving us, they are -'\n\nand she instantly fell asleep. I threw the relic out that night. \n\n'liz fled the next morning, stating the school was the only place where the whispers seemed to fade." }, 
        { 4,  "At first, teachers dismissed the kids' mentions of whispers as figments of their imagination. But then, the drawings began.\n\nChildren started to draw symbols in their notes. Symbols representing concepts they should not have known as this young of an age. They said the symbols protected them from the whispers. When confronted by teachers, kids couldn't recall where they learned about them something else in their eyes - fear. Not of being punished, but of something way beyond that. \n\nHearing these reports from the teachers, I decided to pull Elizabeth from school. My wife and I started to look for houses in the next town. We had had enough of these rituals and relics.\n\n*next entry in the booklet*\n\nThe Nelsons were planning to move out of town. Or at least that's what the preacher told me. He said he had found their crashed car right next to the bridge. He said the scene looked as if they suddenly swerved out of the road to avoid something on the bridge. He brought me their booklet as it was the only salvageable memory of them from the car crash." }, 
        { 5,  "The Nelsons were desperate to leave Hollowstone, but the preacher's story about their car crash didn't add up. He claimed they swerved to avoid something on the bridge, but the tire marks told a different story. They were sharp, sudden-like they were forced off the road.\n\nI went to the bridge myself after hearing the preacher's account. The air felt heavy, oppressive-I felt observed. \n\nWhat haunts me the most is the preacher's calm demeanor as he handed me their booklet. He told me they had 'fulfilled their role' and that I should 'trust the relics' to guide us. But I couldn't shake the thought that he knew more about the crash than he let on. His words didn't feel like consolation-they felt like a warning.\n\nAs time went on, neighbours grew more and more suspicious of the Nelsons crash. Something definitely didn't add up. As the noise grew louder and louder, the preacher decided to call us all at his new church, paid for by every one of us, for an emergency prayer to honor the Nelsons' passing in the tragedy." }, 
        { 6,  "Every pew was filled that night. The preacher's words boomed through the chapel; his voice resolute yet laden with something unspoken. 'The Nelsons were guided by the relics, and so shall we. Their sacrifice is our salvation.'\n\nBut salvation from what?\n\nI looked around me-families clutching each other, eyes fixed forward looking at the speaker, as if by meeting someone else's gaze they might unravel. He stood there, framed by the altar's eerie glow, with the confidence of a man who just made the bargain of his life.\n\nThat's when he spoke of the four houses in east hollow. 'The relics protect us, and their power is spread among the faithful. Trust in them. Seek their guidance.'\n\nIt wasn't what he said, but how he said it. His tone wasn't one of hope-it was of fear. It was as if he knew something bad was coming. More whispers? Another anomaly? As if we hadn't been through enough already..." }, 
        { 7,  "I couldn't get the preacher's words out of my head. \n\n‘Trust the relics. Seek their guidance.' \n\nBut what guidance could they offer when the whispers and the fear only grew stronger? I began to watch him, to follow his movements when no one else dared to question him. That's when I saw him leave the church late one night, clutching something close to his chest. I don't think anyone else noticed, but I couldn't let it go. I followed him, keeping my distance, down the dirt path, along the river, across an old bridge, and into the woods.\n\nIt was there I found it - the island. A place nobody spoke of, as if it wasn't meant to exist. I hid behind the trees, watching as the preacher set something on the ground near an altar-like stone. That's when it happened. \n\nAt first, I thought it was the shadows playing tricks on me. But then his form began to change. His back arched unnaturally, bones cracking and twisting beneath his skin. His face stretched into something horrific, like it was no longer his own. The preacher he... he became a monster." }, 
        { 8,  "*Page appears to fit at the end of the last script*\n\nAnd then the \"preacher\" moved. Faster than I thought anything could. His head snapped toward the sound of a deer grazing nearby. He lunged, and within moments, it was over - the deer reduced to little more than torn flesh and scattered bone. But it wasn't the violence that terrified me the most. \n\nIt was the way he turned afterward, scanning the trees as if he knew I was there. I didn't wait to find out if he'd seen me. I ran, the whispers in my ears turning into deafening roars as I fled. \n\nWhen I returned to town, I realized I couldn't keep this to myself. What I saw was beyond explanation - beyond sanity. I needed answers, or at least someone to tell me I wasn't losing my mind. I thought of the family at the villa in the northeast - the richest people in Hollowstone, with more power and connections than anyone else. If anyone could help, it would be them." }, 
        { 9,  "I remember the night that strange man came to our house. I was in the hallway, listening from behind the wall. He was shouting at my parents, saying wild things about the preacher. \n\nHe told them he'd seen the preacher change into something... unnatural. He said it happened on the island in the woods, where the preacher tore into a deer like some kind of beast. But my parents wouldn't hear any of it. \n\n‘You've lost your mind,' Father said. ‘Ever since the Nelsons' accident, you've been seeing things that aren't there. The preacher is a good man. He's helped this town through its darkest days, and we've had no reason to doubt him. There are no whispers in this house, no signs of his so-called \"evil.\" \n\nBut I knew the man wasn't lying. I knew because I'd seen it too - what the preacher really was. \n\nI'd followed the preacher once, just to see where he went at night. I saw everything. I fear I might have seen things I shouldn't have had. I wrote it all down afterward - every detail, every sound, every horrible sight - because I didn't know what else to do. I hid that booklet somewhere no one would find it. Maybe in the secret room near the balcony or the maze behind the house. I can't remember exactly, but it's there, somewhere." }, 
        { 10, "It wasn't supposed to be scary. I just wanted to see where the preacher went when he left the church. I followed him across the river up north and into the woods. He stopped at this strange circle of stones on the island - altars, maybe? I hid behind a tree, but I could still see everything.\n\nAt first, it looked like he was praying. But then it started. His body stretched, his skin tore in places, and he grew taller, thinner. His face... it wasn't a face anymore.\n\nThat's when another man stepped out of the trees and knelt before him. I didn't recognize him, but he seemed to know the preacher - or the monster he'd become. He then thanked the preacher.\n\nHe said, ‘I see it now. What you've said all along is true. The pain you carry, those scars - it's all to protect us, isn't it?'\n\nFor a moment, I thought the preacher might say something kind back. But then he grabbed the man's head, twisted it, and ripped it clean off. Blood splattered everywhere. I couldn't look away.\n\nThe preacher started reciting something - it sounded like prayers, but the words were all wrong, like a language no one should know. Then he... he started eating. The body, the blood, piece by piece, all of it.\n\nThat's when I realized the truth. The preacher doesn't protect us. He's preparing us. If no one stops him, this town won't just be empty. It'll be gone. Everyone, everything - he'll take it all..." }
    };

    // Awake ensures the singleton pattern is enforced
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }


    public void CollectBookId(int id) 
    {
        booksCollected.Add(id);
        if(difficultyLevels.Contains(InventoryManager.Instance.manuscripts + 1))
        {
            MonsterGlobal.Instance.IncreaseDifficulty();
        }
    } 
    public bool FoundManuscriptId(int id) => booksCollected.Contains(id);
    public string GetScrollContent(int id)
    {
        return scrolls.ContainsKey(id) ? scrolls[id] : ERROR_SCROLL;
    }

    public void CheckProgress()
    {
        deposited++;
        // Update monster difficulty
        if (deposited == InventoryManager.PUZZLE_OBJECTIVE)
        {
            StartCoroutine(RunEndgame());
        }
    }

    public IEnumerator RunEndgame()
    {
        UIManager.Instance.Blackout();

        yield return new WaitForSeconds(2f);

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("SuccessPanel");
    }

}
