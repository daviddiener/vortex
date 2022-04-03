using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Score
{
    public ObjectId _id { set; get; }
    public string username { set; get; }
    public double score { set; get; }

    public Score(string name, double score){
        this.username = name;
        this.score = score;
    }
}
