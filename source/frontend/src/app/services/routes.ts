import {environment} from '../../environments/environment';

export const routes = {
  article: {
    getArticles: environment.apiBasePath + '/v1/Article/GetArticles',
    getArticle: environment.apiBasePath + '/v1/Article/GetArticle',
    addArticle: environment.apiBasePath + '/v1/Article/AddArticle',
    removeArticle: environment.apiBasePath + '/v1/Article/RemoveArticle',
  },
  information: {
    getApplicationName: environment.apiBasePath + '/v1/Information/GetProductName',
  },
  user: {
    getCurrentUser: environment.apiBasePath + '/v1/User/GetCurrentUser',
  }
};
