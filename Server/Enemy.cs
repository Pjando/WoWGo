using System;

public class Enemy {
	
	public long id {get; private set;}

	public string name {get; private set;}

	public float lat {get; private set;}

	public float lon {get; private set;}

	public Enemy(long id, string name, float lat, float lon){
		this.id = id;
		this.name = name;
		this.lat = lat;
		this.lon = lon;
	}
}