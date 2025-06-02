from langchain_community.document_loaders import TextLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.embeddings import HuggingFaceEmbeddings
from langchain_community.embeddings import HuggingFaceEmbeddings
from langchain_community.vectorstores import FAISS
from langchain.chains import RetrievalQA
from transformers import pipeline

# Load and split documents
loader = TextLoader("bank_faq.txt")
documents = loader.load()
text_splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
docs = text_splitter.split_documents(documents)

# Create embeddings and vector store
embedding_model = HuggingFaceEmbeddings()
vectorstore = FAISS.from_documents(docs, embedding_model)

# Set up retriever and QA pipeline
retriever = vectorstore.as_retriever()
qa_pipeline = pipeline("question-answering", model="distilbert-base-cased-distilled-squad")

def get_answer(question):
    relevant_docs = retriever.get_relevant_documents(question, k=5)
    print("Retrieved docs:", [doc.page_content[:100] for doc in relevant_docs])
    
    context = " ".join([doc.page_content for doc in relevant_docs])
    if len(context) > 3000:
        context = context[:3000]  # truncate if too long
    
    result = qa_pipeline(question=question, context=context)
    print("QA result:", result)
    return result['answer']
