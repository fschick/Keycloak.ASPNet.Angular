import {environment} from '../../environments/environment';

export const routes = {
  article: {
    createArticle: environment.apiBasePath + '/v1/Article',
    readArticles: environment.apiBasePath + '/v1/Article',
    readArticle: environment.apiBasePath + '/v1/Article',
    deleteArticle: environment.apiBasePath + '/v1/Article',
  },
  information: {
    getApplicationName: environment.apiBasePath + '/v1/Information/GetProductName',
  },
  user: {
    getCurrentUser: environment.apiBasePath + '/v1/User/GetCurrentUser',
  }
};
