import * as gcs from '@google-cloud/storage';
import * as fs from 'fs';
import * as path from 'path';

export class ObjectAccess{
    // something that I don't have just yet
    storage = new gcs.Storage({keyFilename: "key.json"});
    fileExtension:string  = ".prefab";
    destFolder = __dirname;

    async GetObjectFromCloud(objectId:string, projectId:string):Promise<string> {
        
        const options = {
            destination: this.destFolder + projectId +  objectId + this.fileExtension
        }

        await this.storage
            .bucket(projectId)
            .file(objectId + this.fileExtension)
            .download(options);

        return this.destFolder + projectId + objectId + this.fileExtension;
    }
    
    async RemoveObjectFromLocal(objectId:string, projectId):Promise<void>{
        await fs.unlink(path.resolve(this.destFolder, projectId + objectId + this.fileExtension))
    }

    async DeleteObjectFromCloud(objectId:string, projectId:string):Promise<void>{
        const options = {
            destination: this.destFolder + projectId +  objectId + this.fileExtension
        }

        await this.storage
            .bucket(projectId)
            .file(objectId + this.fileExtension)
            .delete()

    }

    async CreateProjectInCloud(projectId:string){
        await this.storage.createBucket(projectId)
    }

    async DeleteProjectFromCloud(projectId:string){
        await this.storage.bucket(projectId).delete();
    }
}