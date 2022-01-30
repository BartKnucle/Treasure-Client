using System.Collections.Generic;
using FunkySheep.JSON;

namespace FunkySheep.OSM
{
  public class Data
  {
    public List<Way> ways = new List<Way>();
    public List<Relation> relations = new List<Relation>();
    public void AddElement(JSONNode elementJSON)
    {
      switch ((string)elementJSON["type"])
        {
            case "way":
                AddWay(elementJSON);
                break;
            case "relation":
                AddRelation(elementJSON);
                break;
            default:
                break;
        }
    }

    public Way AddWay(JSONNode wayJSON)
    {
      Way way = ways.Find(way => way.id == wayJSON["id"]);

      if (way == null)
      {
        way = new Way(wayJSON["id"]);
      }
      // Add the node id to the nodes list
      JSONArray nodes = wayJSON["nodes"].AsArray;
      JSONArray geometries = wayJSON["geometry"].AsArray;

      for (int i = 0; i < geometries.Count; i++)
      {
        way.nodes.Add(new Node(nodes[i].AsLong, geometries[i]["lat"].AsDouble, geometries[i]["lon"].AsDouble));
      }

      JSONObject tags = wayJSON["tags"].AsObject;

      foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
      {
        way.tags.Add(new Tag(tag.Key, tag.Value));
      }
      
      ways.Add(way);
      return way;
    }

    public Relation AddRelation(JSONNode relationJSON)
    {
      Relation relation = relations.Find(relation => relation.id == relationJSON["id"]);

      if (relation == null)
      {
        relation = new Relation(relationJSON["id"]);
      }

      JSONArray members = relationJSON["members"].AsArray;

      for (int i = 0; i < members.Count; i++)
      {
        members[i].Add("id", members[i]["ref"]);
        switch ((string)members[i]["type"])
        {
            case "way":
                relation.ways.Add(AddWay(members[i]));
                break;
            default:
                break;
        }
      }

      JSONObject tags = relationJSON["tags"].AsObject;

      foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
      {
        relation.tags.Add(new Tag(tag.Key, tag.Value));
      }

      relations.Add(relation);
      return relation;
    }

    public void Merge(Data data)
    {
      foreach (Way way in data.ways)
      {
        ways.Add(way);
      }

      foreach (Relation relation in data.relations)
      {
        relations.Add(relation);
      }
    }
  }

}