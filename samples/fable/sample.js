// export a handful of functions for interop purposes
import { MongoClient } from 'mongodb';

async function connect() {
    const client = new MongoClient("mongodb://192.168.100.5");
    const connection = await client.connect();
    return connection.db("mondocks");
}


export async function savePost(command) {
    const db = await connect();
    const result = await db.command(command);
    return result;
}

export async function findPosts(command) {
    const db = await connect();
    const result = await db.command(command);
    return result?.cursor?.firstBatch;
}