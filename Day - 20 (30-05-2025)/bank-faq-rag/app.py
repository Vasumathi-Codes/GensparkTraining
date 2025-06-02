from fastapi import FastAPI
from pydantic import BaseModel
from rag_pipeline import get_answer

app = FastAPI()

class Question(BaseModel):
    question: str

@app.post("/ask")
def ask_question(q: Question):
    answer = get_answer(q.question)
    return {"answer": answer}

